using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;

public sealed record GetAllInventoryMovementQueryResponse(
    List<InventoryMovementListDto> Movements,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record InventoryMovementListDto(
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
