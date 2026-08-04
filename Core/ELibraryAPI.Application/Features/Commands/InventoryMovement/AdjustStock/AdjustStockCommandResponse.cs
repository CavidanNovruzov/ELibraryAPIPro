namespace ELibraryAPI.Application.Features.Commands.InventoryMovement.AdjustStock;

public sealed record AdjustStockCommandResponse(
    Guid InventoryMovementId,
    Guid ProductId,
    Guid BranchId,
    int NewQuantity
);
