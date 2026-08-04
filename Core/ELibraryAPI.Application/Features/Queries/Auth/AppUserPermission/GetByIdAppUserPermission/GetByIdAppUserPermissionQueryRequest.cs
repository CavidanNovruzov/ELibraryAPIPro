
using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetByIdAppUserPermission;

public sealed record GetByIdAppUserPermissionQueryRequest(Guid UserId, int PermissionId) : IRequest<Result<GetByIdAppUserPermissionQueryResponse>>, ICacheable
{
    public string CacheKey => $"user:permission:{UserId}:{PermissionId}";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(15);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}