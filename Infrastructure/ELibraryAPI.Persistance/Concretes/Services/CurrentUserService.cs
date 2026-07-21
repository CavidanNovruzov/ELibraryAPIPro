
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ELibraryAPI.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? SystemConstants.SystemUserName;

    public Guid UserGuid
    {
        get
        {
            var userId = UserId;
            return Guid.TryParse(userId, out var guid) ? guid : SystemConstants.SystemUserId;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    public bool IsAdmin =>_httpContextAccessor.HttpContext?.User?.IsInRole(RoleNames.Admin) ?? false;
}