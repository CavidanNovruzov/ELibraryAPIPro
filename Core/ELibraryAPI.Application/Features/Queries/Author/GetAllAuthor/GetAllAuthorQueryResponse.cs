

namespace ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;

public sealed record GetAllAuthorQueryResponse(
    List<AuthorListDto> Authors,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AuthorListDto(Guid Id, string FullName, string Country, int BookCount);