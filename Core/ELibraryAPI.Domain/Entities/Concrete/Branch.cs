using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Branch : BaseEntity 
{
    public Branch()
    {
        WorkHours = new HashSet<BranchWorkHours>();
        Stocks = new HashSet<Stock>();
        OutgoingInventoryMovements = new HashSet<InventoryMovement>();
        IncomingInventoryMovements = new HashSet<InventoryMovement>();
    }

    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public virtual ICollection<BranchWorkHours> WorkHours { get; set; }
    public virtual ICollection<Stock> Stocks { get; set; }
    public virtual ICollection<InventoryMovement> OutgoingInventoryMovements { get; set; }
    public virtual ICollection<InventoryMovement> IncomingInventoryMovements { get; set; }
}
