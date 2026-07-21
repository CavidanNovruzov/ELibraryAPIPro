using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;

public sealed record UpdateProfileCommandRequest(
    Guid   UserId,
    string FirstName,
    string LastName,
    string UserName
) : IRequest<Result<UpdateProfileCommandResponse>>;
