using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete.Auth;

public class AppUserPermission : BaseEntity
{
    public AppUserPermission()
    {
    }

    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; } = null!;

    public int PermissionId { get; set; }
    public virtual Permission Permission { get; set; } = null!;

    public Guid? GrantedByUserId { get; set; }
    public virtual AppUser? GrantedByUser { get; set; }

}