using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Permission.GetByIdPermission;

public sealed record GetByIdPermissionQueryRequest(int Id) : IRequest<Result<GetByIdPermissionQueryResponse>>, ICacheable
{
    public string CacheKey => $"permission:detail:{Id}";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(2);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(30);
}