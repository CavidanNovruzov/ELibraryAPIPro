

namespace ELibraryAPI.Application.Abstractions.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    Guid UserGuid { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);

    bool IsAdmin { get; }
}
