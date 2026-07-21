using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete.Auth;

public class Permission : BaseEntity<int>
{
    public Permission()
    {
        UserPermissions = new HashSet<AppUserPermission>();
        RolePermissions = new HashSet<RolePermission>();
    }

    public string Key { get; set; } = null!;

    public bool IsDelegatable { get; set; } = true;


    public virtual ICollection<AppUserPermission> UserPermissions { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}