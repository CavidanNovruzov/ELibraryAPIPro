

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;

public sealed record GetAuthorsByAlphabetQueryResponse(
 List<AuthorAlphabetDto> Authors
);

public sealed record AuthorAlphabetDto(
    Guid Id,
    string FullName,
    int BookCount
);
