using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppUserPermission;

public record SetUserPermissionCommandRequest(Guid UserId, List<int> PermissionIds) : IRequest<Result>;