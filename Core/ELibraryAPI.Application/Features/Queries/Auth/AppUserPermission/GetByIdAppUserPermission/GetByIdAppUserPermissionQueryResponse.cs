namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetByIdAppUserPermission;

public sealed record GetByIdAppUserPermissionQueryResponse(
    int PermissionId,
    string PermissionKey,
    DateTime GrantedDate);