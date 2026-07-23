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
            return Result<CreateOrderCommandResponse>.Failure("Sistemə daxil olmamısınız.", ErrorType.Unauthorized);

        var pendingStatus = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>()
            .GetSingleAsync(s => s.Name == OrderStatusNames.Pending, tracking: false, ct: ct);

        if (pendingStatus == null)
            return Result<CreateOrderCommandResponse>.Failure(
                "Sistem konfiqurasiya xətası: gözləmə statusu tapılmadı.",
                ErrorType.ServerError);

        var shippingMethod = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.ShippingMethod, Guid>()
            .GetSingleAsync(s => s.Id == request.ShippingMethodId, tracking: false, ct: ct);

        if (shippingMethod == null)
            return Result<CreateOrderCommandResponse>.Failure("Çatdırılma metodu tapılmadı.", ErrorType.NotFound);

        var paymentMethodExists = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.PaymentMethod, Guid>()
            .ExistsAsync(p => p.Id == request.PaymentMethodId, tracking: false, ct: ct);

        if (!paymentMethodExists)
            return Result<CreateOrderCommandResponse>.Failure("Ödəniş metodu tapılmadı.", ErrorType.NotFound);

        var basket = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetAll(tracking: true)
            .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.Stocks)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket == null || !basket.BasketItems.Any())
            return Result<CreateOrderCommandResponse>.Failure("Səbətiniz boşdur.", ErrorType.ValidationError);

        var address = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>()
            .GetSingleAsync(a => a.Id == request.UserAddressId && a.UserId == userId, false, ct);

        if (address == null)
            return Result<CreateOrderCommandResponse>.Failure("Çatdırılma ünvanı tapılmadı.", ErrorType.NotFound);

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
                return Result<CreateOrderCommandResponse>.Failure("Promo kod etibarsızdır və ya vaxtı bitib.", ErrorType.ValidationError);

            if (promoCode.UsageCount >= promoCode.UsageLimit)
                return Result<CreateOrderCommandResponse>.Failure("Promo kodun istifadə limiti bitib.", ErrorType.ValidationError);

            var alreadyUsed = await _unitOfWork
                .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
                .GetAll(tracking: false)
                .AnyAsync(o => o.UserId == userId && o.PromoCodeId == promoCode.Id, ct);

            if (alreadyUsed)
                return Result<CreateOrderCommandResponse>.Failure("Bu promo kodu artıq istifadə etmisiniz.", ErrorType.ValidationError);
        }

        decimal total = 0;
        foreach (var item in basket.BasketItems)
        {
            var totalStock = item.Product.Stocks.Sum(s => s.Quantity);
            if (totalStock < item.Quantity)
                return Result<CreateOrderCommandResponse>.Failure(
                    $"'{item.Product.Title}' adlı məhsul stokda yoxdur.", ErrorType.ValidationError);

            total += (item.Product.DiscountPrice ?? item.Product.SalePrice) * item.Quantity;
        }

        if (promoCode != null)
        {
            total -= total * (promoCode.DiscountPercent / 100);
            promoCode.UsageCount++;
        }

        total += shippingMethod.Price;

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
            OrderNumber = $"LF-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}",
            TotalAmount = total,
            PromoCodeId = promoCode?.Id
        };

        var movementWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.InventoryMovement, Guid>();

        // ÖNƏMLİ: order əvvəlcə context-ə əlavə olunur ki, order.Id (client-side Guid generator vasitəsilə)
        // artıq generasiya olunsun. Əvvəllər bu sətir loop-dan SONRA idi və InventoryMovement.OrderId
        // hər zaman Guid.Empty kimi yazılırdı — inventar hərəkətləri sifarişlə əlaqələndirilmirdi.
        await _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>().AddAsync(order, ct);

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
                stock.Decrease(deduction); // invariant (mənfi stok yoxlaması) artıq entity daxilindədir
                remaining -= deduction;

                await movementWriteRepo.AddAsync(new Domain.Entities.Concrete.InventoryMovement
                {
                    ProductId = item.ProductId,
                    FromBranchId = stock.BranchId,
                    ToBranchId = null,
                    OrderId = order.Id, // artıq doğru dəyərdədir
                    Quantity = deduction,
                    Type = InventoryMovementType.Sale,
                    Status = InventoryMovementStatus.Completed
                }, ct);
            }
        }

        _unitOfWork.WriteRepository<Domain.Entities.Concrete.Basket, Guid>().Remove(basket);

        try
        {
            var saved = await _unitOfWork.SaveAsync(ct);
            if (saved > 0)
            {
                await _mediator.Publish(new EntityChangedEvent("order", order.Id), ct);

                foreach (var productId in basket.BasketItems.Select(i => i.ProductId).Distinct())
                {
                    await _mediator.Publish(new EntityChangedEvent("product", productId), ct);
                }

                return Result<CreateOrderCommandResponse>.Success(
                    new CreateOrderCommandResponse(order.Id),
                    "Sifarişiniz uğurla qeydə alındı.");
            }

            return Result<CreateOrderCommandResponse>.Failure("Sifarişiniz emal edilərkən xəta baş verdi.", ErrorType.ServerError);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<CreateOrderCommandResponse>.Failure(
                "Stok və ya promo kodun mövcudluğu ödəniş zamanı dəyişdi. Zəhmət olmasa yenidən cəhd edin.",
                ErrorType.Conflict);
        }
    }
}