using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorById;

public sealed record GetAuthorByIdQueryRequest(Guid Id)
    : IRequest<Result<GetAuthorByIdQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("author", "id", Id);

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}