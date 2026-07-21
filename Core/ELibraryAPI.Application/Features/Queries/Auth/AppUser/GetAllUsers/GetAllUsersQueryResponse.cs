namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetAllUsers;

public sealed record UserSummaryDto(
    Guid   Id,
    string FullName,
    string Email,
    string UserName,
    bool   EmailConfirmed
);

public sealed record GetAllUsersQueryResponse(
    List<UserSummaryDto> Users,
    int TotalCount
);
