
using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.CreateRole;

public sealed record CreateRoleCommandRequest(string Name) : IRequest<Result<CreateRoleCommandResponse>>;
