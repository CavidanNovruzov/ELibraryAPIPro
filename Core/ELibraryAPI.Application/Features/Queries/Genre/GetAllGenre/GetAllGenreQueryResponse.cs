namespace ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;

public sealed record GetAllGenreQueryResponse(
    List<GenreListDto> Genres,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record GenreListDto(Guid Id, string Name, int ProductCount);
