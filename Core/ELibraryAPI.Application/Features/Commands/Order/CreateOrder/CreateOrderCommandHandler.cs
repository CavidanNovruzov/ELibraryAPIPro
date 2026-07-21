using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.CreateOrder;

public sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommandRequest, Result<CreateOrderCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public async Task<Result<CreateOrderCommandResponse>> Handle(
        CreateOrderCommandRequest request,
        CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<CreateOrderCommandResponse>.Failure("User is not authenticated.", ErrorType.Unauthorized);

        var pendingStatus = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>()
            .GetSingleAsync(s => s.Name == OrderStatusNames.Pending, tracking: false, ct: ct);

        if (pendingStatus == null)
            return Result<CreateOrderCommandResponse>.Failure(
                "System configuration error: Pending order status not found.",
                ErrorType.ServerError);

        var basket = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetAll(tracking: true)
            .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.Stocks)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket == null || !basket.BasketItems.Any())
            return Result<CreateOrderCommandResponse>.Failure("Your basket is empty.", ErrorType.ValidationError);

        var address = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>()
            .GetSingleAsync(a => a.Id == request.UserAddressId && a.UserId == userId, false, ct);

        if (address == null)
            return Result<CreateOrderCommandResponse>.Failure("Shipping address not found.", ErrorType.NotFound);

        Domain.Entities.Concrete.PromoCode? promoCode = null;
        if (!string.IsNullOrWhiteSpace(request.PromoCode))
        {
            promoCode = await _unitOfWork
                .ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>()
                .GetSingleAsync(
                    x => x.Code == request.PromoCode.Trim()
                         && x.IsActive && !x.IsDeleted
                         && x.StartDate <= DateTime.UtcNow
                         && x.EndDate >= DateTime.UtcNow,
                    tracking: true, ct: ct);

            if (promoCode == null)
                return Result<CreateOrderCommandResponse>.Failure("Invalid or expired promo code.", ErrorType.ValidationError);

            if (promoCode.UsageCount >= promoCode.UsageLimit)
                return Result<CreateOrderCommandResponse>.Failure("Promo code usage limit reached.", ErrorType.ValidationError);

            var alreadyUsed = await _unitOfWork
                .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
                .GetAll(tracking: false)
                .AnyAsync(o => o.UserId == userId && o.PromoCodeId == promoCode.Id, ct);

            if (alreadyUsed)
                return Result<CreateOrderCommandResponse>.Failure("You have already used this promo code.", ErrorType.ValidationError);
        }

        decimal total = 0;
        foreach (var item in basket.BasketItems)
        {
            var totalStock = item.Product.Stocks.Sum(s => s.Quantity);
            if (totalStock < item.Quantity)
                return Result<CreateOrderCommandResponse>.Failure(
                    $"Product '{item.Product.Title}' is out of stock.", ErrorType.ValidationError);

            total += (item.Product.DiscountPrice ?? item.Product.SalePrice) * item.Quantity;
        }

        if (promoCode != null)
        {
            total -= total * (promoCode.DiscountPercent / 100);
            promoCode.UsageCount++;
        }

        var order = new Domain.Entities.Concrete.Order
        {
            UserId = userId,
            OrderStatusId = pendingStatus.Id,
            PaymentMethodId = request.PaymentMethodId,
            ShippingMethodId = request.ShippingMethodId,
            ShippingAddressId = address.Id,
            ShippingAddressLine = address.AddressLine,
            ShippingCity = address.City,
            OrderNote = request.OrderNote,
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}",
            TotalAmount = total,
            PromoCodeId = promoCode?.Id
        };

        var movementWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.InventoryMovement, Guid>();

        foreach (var item in basket.BasketItems)
        {
            order.OrderItems.Add(new Domain.Entities.Concrete.OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product.DiscountPrice ?? item.Product.SalePrice
            });

            int remaining = item.Quantity;
            foreach (var stock in item.Product.Stocks
                         .Where(s => s.Quantity > 0)
                         .OrderByDescending(s => s.Quantity))
            {
                if (remaining <= 0) break;
                int deduction = Math.Min(stock.Quantity, remaining);
                stock.Quantity -= deduction;
                remaining -= deduction;

                await movementWriteRepo.AddAsync(new Domain.Entities.Concrete.InventoryMovement
                {
                    ProductId = item.ProductId,
                    FromBranchId = stock.BranchId,
                    ToBranchId = null,
                    OrderId = order.Id,
                    Quantity = deduction,
                    Type = InventoryMovementType.Sale,
                    Status = InventoryMovementStatus.Completed
                }, ct);
            }
        }

        await _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>().AddAsync(order, ct);
        _unitOfWork.WriteRepository<Domain.Entities.Concrete.Basket, Guid>().Remove(basket);

        try
        {
            var saved = await _unitOfWork.SaveAsync(ct);
            if (saved > 0)
            {
                await _mediator.Publish(new EntityChangedEvent("order", order.Id), ct);

                return Result<CreateOrderCommandResponse>.Success(
                    new CreateOrderCommandResponse(order.Id),
                    "Order has been placed successfully.");
            }

            return Result<CreateOrderCommandResponse>.Failure("An error occurred while processing your order.", ErrorType.BadRequest);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<CreateOrderCommandResponse>.Failure(
                "Stock availability changed during checkout. Please try again.",
                ErrorType.Conflict);
        }
    }
}