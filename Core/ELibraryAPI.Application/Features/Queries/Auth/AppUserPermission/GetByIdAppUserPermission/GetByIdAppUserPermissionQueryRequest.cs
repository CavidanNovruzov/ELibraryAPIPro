
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetByIdAppUserPermission;

public sealed record GetByIdAppUserPermissionQueryRequest(Guid UserId, int PermissionId) : IRequest<Result<GetByIdAppUserPermissionQueryResponse>>;