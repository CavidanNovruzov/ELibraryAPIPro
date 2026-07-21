namespace ELibraryAPI.Application.Features.Queries.Permission.GetAllPermission;

public sealed record GetAllPermissionQueryResponse(
    int Id,
    string Key,
    bool IsDelegatable,
    DateTime CreatedDate);