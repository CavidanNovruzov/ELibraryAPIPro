using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;

public sealed record GetMyProfileQueryRequest()
    : IRequest<Result<GetMyProfileQueryResponse>>, ICacheable
{
    public string CacheKey => "user:profile:me";

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(20);

    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}
