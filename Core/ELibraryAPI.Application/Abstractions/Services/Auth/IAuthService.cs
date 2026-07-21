using ELibraryAPI.Application.Dtos.Auth;
using ELibraryAPI.Application.Responses;


namespace ELibraryAPI.Application.Abstractions.Services.Auth;

public interface IAuthService
{
    Task<Result<Guid>> RegisterAsync(RegistrRequest request, CancellationToken ct = default);
    Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request,CancellationToken ct=default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken ct = default);
    Task<HashSet<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default);
}
