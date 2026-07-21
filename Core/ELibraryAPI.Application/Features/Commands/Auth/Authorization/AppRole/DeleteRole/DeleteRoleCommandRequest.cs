

using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.DeleteRole;

public sealed record DeleteRoleCommandRequest(Guid Id) : IRequest<Result>;
