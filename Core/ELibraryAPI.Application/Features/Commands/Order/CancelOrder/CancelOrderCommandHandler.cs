using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.CancelOrder;

public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CancelOrderCommandRequest request, CancellationToken ct)
    {
        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: true)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Stocks)
            .FirstOrDefaultAsync(o => o.Id == request.Id && !o.IsDeleted, ct);

        if (order == null) return Result.Failure("Sifariş tapılmadı..");

        var movements = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.InventoryMovement, Guid>()
            .GetWhere(m => m.OrderId == request.Id && m.Type == InventoryMovementType.Sale, tracking: false)
            .ToListAsync(ct);

        var movementWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.InventoryMovement, Guid>();

        foreach (var item in order.OrderItems)
        {
            var itemMovements = movements.Where(m => m.ProductId == item.ProductId).ToList();
            foreach (var movement in itemMovements)
            {
                var stock = item.Product?.Stocks.FirstOrDefault(s => s.BranchId == movement.FromBranchId);
                if (stock != null)
                {
                    stock.Quantity += movement.Quantity;

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
        return Result.Success("Sifariş uğurla ləğv edildi.");
    }
}
