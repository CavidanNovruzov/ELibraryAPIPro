using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching; 
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;

public sealed record GetAllProductQueryRequest(
    int Page = 1,
    int Size = 20,
    string? Search = null,
    Guid? CategoryId = null,
    Guid? SubCategoryId = null,
    Guid? AuthorId = null,
    Guid? GenreId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = null,
    bool? IsInDiscount = null
) : IRequest<Result<GetAllProductQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create(
        "product",
        "list",
        Search,
        CategoryId,
        SubCategoryId,
        AuthorId,
        GenreId,
        $"{MinPrice}-{MaxPrice}",
        IsInDiscount,
        SortBy,
        Page,
        Size
    );

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(3);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(1);
}