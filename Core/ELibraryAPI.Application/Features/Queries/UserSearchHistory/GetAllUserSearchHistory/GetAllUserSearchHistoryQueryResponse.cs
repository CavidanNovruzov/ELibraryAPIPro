namespace ELibraryAPI.Application.Features.Queries.UserSearchHistory.GetAllUserSearchHistory;

public sealed record GetAllUserSearchHistoryQueryResponse(
    List<UserSearchHistoryListDto> SearchHistories
);

public sealed record UserSearchHistoryListDto(
    Guid Id,
    Guid UserId,
    string SearchQuery,
    DateTime CreatedDate
);
