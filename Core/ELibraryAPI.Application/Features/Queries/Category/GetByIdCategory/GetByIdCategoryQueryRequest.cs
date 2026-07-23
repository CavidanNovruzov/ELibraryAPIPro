using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;

public sealed record GetByIdCategoryQueryRequest(Guid Id)
    : IRequest<Result<GetByIdCategoryQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("category", "id", Id);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}