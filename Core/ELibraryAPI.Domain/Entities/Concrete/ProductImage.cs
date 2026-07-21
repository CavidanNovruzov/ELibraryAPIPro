using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class ProductImage : BaseEntity
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
}