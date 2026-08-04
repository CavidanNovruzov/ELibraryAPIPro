using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetAllAppUserPermission;

public sealed record GetAllAppUserPermissionQueryRequest(Guid UserId) : IRequest<Result<List<GetAllAppUserPermissionQueryResponse>>>, ICacheable
{
    public string CacheKey => $"user:permissions:all:{UserId}";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(15);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}