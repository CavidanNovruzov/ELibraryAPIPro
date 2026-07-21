namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorById;

public sealed record GetAuthorByIdQueryResponse(
    Guid Id,
    string FullName,
    string Biography,
    string Country,
    List<AuthorBookDto> Books
);

public sealed record AuthorBookDto(
    Guid Id,
    string Title,
    decimal Price, 
    string? ImageUrl
);
