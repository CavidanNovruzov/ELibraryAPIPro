using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch;

public sealed record GetCategorySearchQueryRequest(
    string SearchTerm,
    int Page = 1,
    int Size = 10
) : IRequest<Result<GetCategorySearchQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("category", "search", SearchTerm.Trim().ToLower(), Page, Size);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(5);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(1);
}