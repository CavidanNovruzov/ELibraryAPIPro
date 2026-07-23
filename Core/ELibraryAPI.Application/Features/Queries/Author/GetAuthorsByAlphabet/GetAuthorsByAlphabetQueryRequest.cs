using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;

public sealed record GetAuthorsByAlphabetQueryRequest(char Letter)
    : IRequest<Result<GetAuthorsByAlphabetQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("author", "alphabet", char.ToLower(Letter));

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(1);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(15);
}