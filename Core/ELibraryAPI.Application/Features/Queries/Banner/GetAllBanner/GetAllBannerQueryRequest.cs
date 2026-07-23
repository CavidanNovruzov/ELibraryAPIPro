using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;

public sealed record GetAllBannerQueryRequest(int Page = 1, int Size = 10)
    : IRequest<Result<GetAllBannerQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("banner", "list", Page, Size);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}