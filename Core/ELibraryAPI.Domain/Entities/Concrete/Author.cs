using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Author : BaseEntity
{
    public Author()
    {
        ProductAuthors = new HashSet<ProductAuthor>();
    }

    public string FullName { get; set; } = null!;
    public string? Biography { get; set; }
    public string? ImagePath { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<ProductAuthor> ProductAuthors { get; set; }
}
