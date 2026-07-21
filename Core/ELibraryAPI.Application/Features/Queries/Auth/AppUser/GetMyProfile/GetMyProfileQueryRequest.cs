using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;

public sealed record GetMyProfileQueryRequest(Guid UserId) : IRequest<Result<GetMyProfileQueryResponse>>;
