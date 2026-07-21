namespace ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetAllRolePermission;

public sealed record RolePermissionListDto(
    Guid RoleId,
    string RoleName,
    int PermissionId,
    string PermissionKey
);

public sealed record GetAllRolePermissionQueryResponse(
    List<RolePermissionListDto> RolePermissions
);