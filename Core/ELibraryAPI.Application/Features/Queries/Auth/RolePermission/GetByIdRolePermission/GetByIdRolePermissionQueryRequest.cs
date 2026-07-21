using ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetByIdRolePermission;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.RolePermission.GetByIdRolePermission;

public sealed record GetByIdRolePermissionQueryRequest(Guid RoleId) : IRequest<Result<GetByIdRolePermissionQueryResponse>>;
