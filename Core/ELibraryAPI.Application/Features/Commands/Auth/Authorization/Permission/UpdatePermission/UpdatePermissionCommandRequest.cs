using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.UpdatePermission;

public sealed record UpdatePermissionCommandRequest(
    int Id,
    string Key
) : IRequest<Result<UpdatePermissionCommandResponse>>;
