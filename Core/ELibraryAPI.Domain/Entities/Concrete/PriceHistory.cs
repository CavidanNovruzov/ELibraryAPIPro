using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class PriceHistory : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }

    public string? ChangeReason { get; set; }
}
