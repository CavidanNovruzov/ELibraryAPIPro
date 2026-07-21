using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetUserById;

public sealed record GetUserByIdQueryRequest(Guid UserId) : IRequest<Result<GetUserByIdQueryResponse>>;
