namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;

public sealed record GetMyProfileQueryResponse(
    Guid   Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    bool   EmailConfirmed,
    List<string> Roles
);
