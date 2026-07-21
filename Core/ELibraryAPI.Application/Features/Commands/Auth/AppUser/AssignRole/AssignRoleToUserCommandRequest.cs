using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.AssignRole;

public record AssignRoleToUserCommandRequest(Guid UserId, string RoleName) : IRequest<Result>;