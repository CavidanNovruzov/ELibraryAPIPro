using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using ELibraryAPI.Application.UnitOfWork;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenService(
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> jwtOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _jwtOptions = jwtOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> CreateRefreshTokenAsync(AppUser user, CancellationToken ct = default)
    {
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var hashedToken = HashToken(rawToken);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = hashedToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpirationMinutes),
            CreatedByIp = GetClientIp(),
            IsRevoked = false
        };

        await _unitOfWork.RefreshTokenWrite.AddAsync(refreshToken, ct);
        await _unitOfWork.SaveAsync(ct);

        return rawToken;
    }

    public async Task<AppUser?> ValidateAndConsumeAsync(string rawToken, CancellationToken ct = default)
    {
        var hashedToken = HashToken(rawToken);
        var refreshToken = await _unitOfWork.RefreshTokenRead.GetByTokenWithUserAsync(hashedToken, ct);

        if (refreshToken == null || refreshToken.IsRevoked)
            return null;

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            _unitOfWork.RefreshTokenWrite.Update(refreshToken);
            await _unitOfWork.SaveAsync(ct);
            return null;
        }

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = GetClientIp();

        _unitOfWork.RefreshTokenWrite.Update(refreshToken);
        await _unitOfWork.SaveAsync(ct);

        return refreshToken.User;
    }

    private string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    private string GetClientIp()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return "Unknown";

        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
        {
            var firstIp = forwarded.ToString().Split(',')[0].Trim();
            if (!string.IsNullOrEmpty(firstIp)) return firstIp;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
