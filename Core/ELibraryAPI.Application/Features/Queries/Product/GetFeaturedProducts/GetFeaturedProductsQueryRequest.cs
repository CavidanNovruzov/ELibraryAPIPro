using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Product.GetFeaturedProducts;

public sealed record GetFeaturedProductsQueryRequest : IRequest<Result<List<GetFeaturedProductsQueryResponse>>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("product", "featured");
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}

