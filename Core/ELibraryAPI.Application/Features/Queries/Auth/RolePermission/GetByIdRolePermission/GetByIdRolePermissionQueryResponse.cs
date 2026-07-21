namespace ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetByIdRolePermission;

public sealed record GetByIdRolePermissionQueryResponse(
    Guid RoleId,
    string RoleName,
    List<int> SelectedPermissionIds);