using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistance.Concrete.Repositories;
using ELibraryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Auth;

public class RefreshTokenReadRepository : ReadRepository<RefreshToken, Guid>, IRefreshTokenReadRepository
{
    public RefreshTokenReadRepository(ELibraryDbContext context) : base(context) { }
    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken ct = default)
    {
        return await GetSingleAsync(
            method: rt => rt.TokenHash == token,
            tracking: false,
            ct: ct,
            includes: rt => rt.User
        );
    }
}
