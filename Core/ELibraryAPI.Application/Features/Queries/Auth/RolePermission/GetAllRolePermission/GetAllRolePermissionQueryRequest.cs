
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetAllRolePermission;

public sealed record GetAllRolePermissionQueryRequest : IRequest<Result<GetAllRolePermissionQueryResponse>>;