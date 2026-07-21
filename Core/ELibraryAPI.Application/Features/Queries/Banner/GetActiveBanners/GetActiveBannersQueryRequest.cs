using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Banner.GetActiveBanners;

public sealed record GetActiveBannersQueryRequest : IRequest<Result<List<GetActiveBannersQueryResponse>>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("banner", "active");
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(2);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(15);
}
