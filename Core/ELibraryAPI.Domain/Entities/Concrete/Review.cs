using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class Review : BaseEntity, IOwnership 
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public int Rating { get; set; }

    public bool IsApproved { get; set; } = false;

}