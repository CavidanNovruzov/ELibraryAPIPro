namespace ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;

public sealed record GetByIdGenreQueryResponse(
    GenreDetailDto Genre
);

public sealed record GenreDetailDto(
    Guid Id,
    string Name,
    int ProductCount
);
