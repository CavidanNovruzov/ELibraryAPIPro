using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.Shared.Models;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.CancelOrder;

public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;
    private readonly IEventBus _eventBus;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPublisher publisher, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _publisher = publisher;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CancelOrderCommandRequest request, CancellationToken ct)
    {
        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: true)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Stocks)
            .FirstOrDefaultAsync(o => o.Id == request.Id && !o.IsDeleted, ct);

        if (order == null) return Result.Failure("Sifariş tapılmadı..");

        if (order.UserId != _currentUserService.UserGuid && !_currentUserService.IsAdmin) 
            return Result.Forbidden("Bu sifarişi ləğv etmək üçün icazəniz yoxdur.");

        var movements = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.InventoryMovement, Guid>()
            .GetWhere(m => m.OrderId == request.Id && m.Type == InventoryMovementType.Sale, tracking: false)
            .ToListAsync(ct);

        var movementWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.InventoryMovement, Guid>();

        var backInStockProductsIds = new HashSet<Guid>();

        foreach (var item in order.OrderItems)
        {
            var itemMovements = movements.Where(m => m.ProductId == item.ProductId).ToList();
            foreach (var movement in itemMovements)
            {
                var stock = item.Product?.Stocks.FirstOrDefault(s => s.BranchId == movement.FromBranchId);
                if (stock != null)
                {
                    bool wasOutOfStock = stock.Quantity == 0;
                    stock.Quantity += movement.Quantity;

                    if (wasOutOfStock && stock.Quantity > 0)
                        backInStockProductsIds.Add(item.ProductId);

                    await movementWriteRepo.AddAsync(new Domain.Entities.Concrete.InventoryMovement
                    {
                        ProductId = item.ProductId,
                        FromBranchId = movement.FromBranchId,
                        ToBranchId = null,
                        OrderId = order.Id,
                        Quantity = movement.Quantity,
                        Type = InventoryMovementType.Return,
                        Status = InventoryMovementStatus.Completed
                    }, ct);
                }
            }
        }
        var cancelledStatus = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>()
            .GetSingleAsync(x => x.Name == OrderStatusNames.Cancelled, tracking: false, ct: ct);

        if (cancelledStatus != null) order.OrderStatusId = cancelledStatus.Id;

        await _unitOfWork.SaveAsync(ct);


        await _publisher.Publish(new EntityChangedEvent("order", order.Id), ct);
        await _eventBus.PublishAsync(new OrderStatusChangedMessage(order.Id, OrderStatusNames.Cancelled), ct);

        foreach (var productId in backInStockProductsIds)
        {
          await  _publisher.Publish(new ProductBackInStockEvent(productId), ct);
        }

        return Result.Success("Sifariş uğurla ləğv edildi.");
    }
}
