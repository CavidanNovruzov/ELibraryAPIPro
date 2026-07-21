using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.RolePermission;

public record SetRolePermissionsCommandRequest(Guid RoleId, List<int> PermissionIds) : IRequest<Result>;