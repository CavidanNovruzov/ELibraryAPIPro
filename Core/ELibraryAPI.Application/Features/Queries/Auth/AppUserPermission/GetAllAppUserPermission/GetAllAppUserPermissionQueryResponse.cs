namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetAllAppUserPermission;

public sealed record GetAllAppUserPermissionQueryResponse(
    int PermissionId,
    string PermissionKey,
    DateTime GrantedDate);