using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.CreatePermission;

public sealed record CreatePermissionCommandRequest(
    string Key
) : IRequest<Result<CreatePermissionCommandResponse>>;
