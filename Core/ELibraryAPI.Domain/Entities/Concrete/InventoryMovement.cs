using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class InventoryMovement : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid FromBranchId { get; set; }
    public virtual Branch FromBranch { get; set; } = null!;

    public Guid? ToBranchId { get; set; }
    public virtual Branch? ToBranch { get; set; }

    public Guid? OrderId { get; set; }
    public virtual Order? Order { get; set; }

    public int Quantity { get; set; }

    public InventoryMovementType Type { get; set; }
    public InventoryMovementStatus Status { get; set; } = InventoryMovementStatus.Completed;
}