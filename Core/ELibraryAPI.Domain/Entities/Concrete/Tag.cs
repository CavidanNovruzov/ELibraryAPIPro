using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
    public virtual ICollection<ProductTag> ProductTags { get; set; } = new HashSet<ProductTag>();
}
