using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class CoverType : BaseEntity
{
    public string Name { get; set; } = null!;


    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}