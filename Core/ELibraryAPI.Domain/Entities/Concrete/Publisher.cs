using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Publisher : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }


    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}