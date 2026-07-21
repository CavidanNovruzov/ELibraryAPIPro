using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangePassword;

public sealed record ChangePasswordCommandRequest(
    Guid   UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Result>;
