using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetAllAppUserPermission;

public sealed record GetAllAppUserPermissionQueryRequest(Guid UserId) : IRequest<Result<List<GetAllAppUserPermissionQueryResponse>>>;