
using ELibraryAPI.Domain.Entities.Concrete.Auth;


namespace ELibraryAPI.Application.Abstractions.Repositories.Auth
{
    public interface IRolePermissionReadRepository : IReadRepository<RolePermission, Guid> { }
}
