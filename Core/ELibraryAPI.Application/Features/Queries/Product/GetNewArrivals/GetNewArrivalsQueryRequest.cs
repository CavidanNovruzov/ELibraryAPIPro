using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Product.GetNewArrivals;

public sealed record GetNewArrivalsQueryRequest : IRequest<Result<List<GetNewArrivalsQueryResponse>>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("product", "newarrivals");

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(15);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(3);
}
