using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;

public sealed record GetAllCategoryQueryRequest(int Page = 1, int Size = 10)
    : IRequest<Result<GetAllCategoryQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("category", "list", Page, Size);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}