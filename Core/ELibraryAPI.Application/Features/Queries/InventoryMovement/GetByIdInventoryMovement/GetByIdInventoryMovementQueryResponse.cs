using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetByIdInventoryMovement;

public sealed record GetByIdInventoryMovementQueryResponse(
    InventoryMovementDetailDto Movement
);

public sealed record InventoryMovementDetailDto(
    Guid         Id,
    Guid         ProductId,
    string       ProductTitle,
    Guid         FromBranchId,
    string       FromBranchName,
    Guid?        ToBranchId,
    string?      ToBranchName,
    int          Quantity,
    InventoryMovementType Type,
    InventoryMovementStatus Status,
    DateTime     CreatedDate
);
