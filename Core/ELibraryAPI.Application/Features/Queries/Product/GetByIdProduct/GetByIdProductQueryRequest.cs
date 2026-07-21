using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching; 
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;

public sealed record GetByIdProductQueryRequest(Guid Id)
    : IRequest<Result<GetByIdProductQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("product", "id", Id);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(10);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5); 
}