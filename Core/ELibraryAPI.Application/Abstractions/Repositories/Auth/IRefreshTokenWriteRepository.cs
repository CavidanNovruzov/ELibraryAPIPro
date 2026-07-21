using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Abstractions.Repositories.Auth;

public interface IRefreshTokenWriteRepository : IWriteRepository<RefreshToken, Guid>
{
    Task<bool> DeleteByTokenAsync(string tokenHash, CancellationToken ct = default);
}
