using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Domain.Entities.Concrete.Auth;

public class RolePermission : BaseEntity
{
    public RolePermission()
    {
    }

    public Guid RoleId { get; set; }
    public virtual AppRole Role { get; set; } = null!;

    public int PermissionId { get; set; }
    public virtual Permission Permission { get; set; } = null!;

}