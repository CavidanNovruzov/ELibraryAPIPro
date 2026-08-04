using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.AppRole.GetAllRoles;

public sealed record GetAllRolesQueryRequest() : IRequest<Result<List<GetAllRolesQueryResponse>>>, ICacheable
{
    public string CacheKey => "roles:all";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(1);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(20);
}