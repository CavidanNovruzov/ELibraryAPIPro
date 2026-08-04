using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetByIdRolePermission;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.RolePermission.GetByIdRolePermission;

public sealed record GetByIdRolePermissionQueryRequest(Guid RoleId) : IRequest<Result<GetByIdRolePermissionQueryResponse>>, ICacheable
{
    public string CacheKey => $"role:permission:detail:{RoleId}";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(1);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(20);
}