using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete.Auth;


public class AppRole : IdentityRoleBaseEntity<Guid>
{
    public AppRole() : base()
    {
        RolePermissions = new HashSet<RolePermission>();
    }

    public AppRole(string roleName) : base(roleName)
    {
        RolePermissions = new HashSet<RolePermission>();
    }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}
