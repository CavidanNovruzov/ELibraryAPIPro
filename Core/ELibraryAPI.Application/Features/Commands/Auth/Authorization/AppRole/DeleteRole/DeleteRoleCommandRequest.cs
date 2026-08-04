

using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.DeleteRole;

public sealed record DeleteRoleCommandRequest(Guid Id) : IRequest<Result>;
