using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Auth;

public class UserPermissionWriteRepository : WriteRepository<AppUserPermission, Guid>, IUserPermissionWriteRepository
{
    public UserPermissionWriteRepository(ELibraryDbContext context) : base(context)
    {
    }
}