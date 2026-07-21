using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Domain.Entities.Concrete;

public class UserSearchHistory : BaseEntity, IOwnership
{
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; } = null!;

    public string SearchQuery { get; set; } = null!;
}