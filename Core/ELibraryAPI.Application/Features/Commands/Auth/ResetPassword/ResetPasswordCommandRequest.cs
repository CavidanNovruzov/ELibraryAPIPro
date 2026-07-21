using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.ResetPassword;

public sealed record ResetPasswordCommandRequest(
    Guid   UserId,
    string Token,
    string NewPassword
) : IRequest<Result>;
