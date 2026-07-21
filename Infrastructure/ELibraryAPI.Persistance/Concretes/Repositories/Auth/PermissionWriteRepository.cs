using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Auth;

public class PermissionWriteRepository : WriteRepository<Permission,int>, IPermissionWriteRepository
{
    public PermissionWriteRepository(ELibraryDbContext context) : base(context)
    {
    }
}
