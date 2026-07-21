using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.DeletePermission;

public sealed record DeletePermissionCommandRequest(int Id) : IRequest<Result>;
