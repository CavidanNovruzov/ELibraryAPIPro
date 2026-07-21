namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetUserById;

public sealed record GetUserByIdQueryResponse(
    Guid         Id,
    string       FirstName,
    string       LastName,
    string       UserName,
    string       Email,
    bool         EmailConfirmed,
    List<string> Roles,
    List<string> Permissions
);
