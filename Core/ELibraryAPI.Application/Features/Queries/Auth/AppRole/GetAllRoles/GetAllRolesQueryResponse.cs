namespace ELibraryAPI.Application.Features.Queries.AppRole.GetAllRoles;

public sealed record GetAllRolesQueryResponse(
    Guid Id,
    string Name,
    bool IsActive,
    DateTime CreatedDate,
    string CreatedBy);