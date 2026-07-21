using ELibraryAPI.Domain.Entities.Common;


namespace ELibraryAPI.Domain.Entities.Concrete;

public class ProductTag : BaseEntity
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
}