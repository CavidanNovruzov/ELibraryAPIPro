using ELibraryAPI.Application.Responses;
using MediatR;
using System.Text.Json.Serialization;


namespace ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.UpdateRole;

public sealed record UpdateRoleCommandRequest(
   [property:JsonIgnore] Guid Id,
   string Name) : IRequest<Result>;
