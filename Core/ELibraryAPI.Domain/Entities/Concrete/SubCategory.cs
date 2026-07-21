using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class SubCategory : BaseEntity
{
    public string Name { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}