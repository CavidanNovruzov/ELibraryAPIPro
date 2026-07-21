using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistance.Concrete.Repositories;
using ELibraryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Auth;

public class UserPermissionReadRepository : ReadRepository<AppUserPermission,Guid>, IUserPermissionReadRepository
{
    public UserPermissionReadRepository(ELibraryDbContext context) : base(context)
    {
    }
}