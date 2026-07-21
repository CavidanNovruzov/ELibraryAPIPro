using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Persistance.Concrete.Repositories.Auth;

public class RefreshTokenWriteRepository : WriteRepository<RefreshToken, Guid>, IRefreshTokenWriteRepository
{
    private readonly ELibraryDbContext _context;
    public RefreshTokenWriteRepository(ELibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> DeleteByTokenAsync(string token, CancellationToken ct = default)
    {
        var affectedRows = await _context.Set<RefreshToken>()
            .Where(rt => rt.TokenHash == token)
            .ExecuteDeleteAsync(ct);

        return affectedRows > 0;
    }
}