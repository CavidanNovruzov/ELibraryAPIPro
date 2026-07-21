using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;


    public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
}