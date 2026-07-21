namespace ELibraryAPI.Application.Features.Queries.Permission.GetByIdPermission;

public sealed record GetByIdPermissionQueryResponse(
    int Id,
    string Key,
    bool IsDelegatable,
    DateTime CreatedDate);