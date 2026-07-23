using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;

public sealed record GetAllAuthorQueryRequest(
    int Page = 1,
    int Size = 10,
    string? SearchTerm = null
) : IRequest<Result<GetAllAuthorQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create(
        "author",
        "list",
        Page,
        Size,
        string.IsNullOrWhiteSpace(SearchTerm) ? "all" : SearchTerm.Trim().ToLower()
    );

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}