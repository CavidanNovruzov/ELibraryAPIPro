using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.InventoryMovement.AdjustStock;

public sealed class AdjustStockCommandHandler
    : IRequestHandler<AdjustStockCommandRequest, Result<AdjustStockCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public AdjustStockCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<AdjustStockCommandResponse>> Handle(
        AdjustStockCommandRequest request, CancellationToken ct)
    {
        var stockWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Stock, Guid>();
        var movementWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.InventoryMovement, Guid>();

        var stock = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Stock, Guid>()
            .GetAll(tracking: true)
            .FirstOrDefaultAsync(s => s.ProductId == request.ProductId && s.BranchId == request.BranchId, ct);

        if (stock == null)
            return Result<AdjustStockCommandResponse>.Failure("Bu məhsul üçün seçilmiş filialda stok qeydi tapılmadı.");

        var newQuantity = stock.Quantity + request.QuantityDelta;
        if (newQuantity < 0)
            return Result<AdjustStockCommandResponse>.Failure(
                $"Korreksiyadan sonra stok mənfi olacaq (Mövcud: {stock.Quantity}, Dəyişiklik: {request.QuantityDelta}).");

        stock.Quantity = newQuantity;
        stockWriteRepo.Update(stock);

        var movement = new Domain.Entities.Concrete.InventoryMovement
        {
            ProductId = request.ProductId,
            FromBranchId = request.BranchId,
            ToBranchId = null,
            OrderId = null,
            Quantity = Math.Abs(request.QuantityDelta),
            Type = request.Type,
            Status = InventoryMovementStatus.Completed
        };

        await movementWriteRepo.AddAsync(movement, ct);

        await _unitOfWork.SaveAsync(ct);

        // Eyni EntityChangedEvent konvensiyası: Product keşini təmizlə.
        await _mediator.Publish(new EntityChangedEvent("product", request.ProductId), ct);

        // Stok yenidən müsbətə keçdisə, "back in stock" bildirişini tetiklə —
        // CreateStock/UpdateStock handler-lərində olan konvensiya ilə eynidir.
        if (stock.Quantity > 0 && request.QuantityDelta > 0)
            await _mediator.Publish(new ProductBackInStockEvent(request.ProductId), ct);

        return Result<AdjustStockCommandResponse>.Success(
            new AdjustStockCommandResponse(movement.Id, request.ProductId, request.BranchId, stock.Quantity),
            "Stok korreksiyası uğurla qeydə alındı.");
    }
}
