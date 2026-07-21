using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Application.Abstractions.Repositories.Auth;

public interface IRefreshTokenReadRepository : IReadRepository<RefreshToken, Guid>
{
    Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken ct = default);
}