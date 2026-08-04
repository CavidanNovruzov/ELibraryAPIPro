using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetUserById;

public sealed record GetUserByIdQueryRequest(Guid UserId)
    : IRequest<Result<GetUserByIdQueryResponse>>, ICacheable
{
    public string CacheKey => $"user:detail:{UserId}";

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(15);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}
