namespace ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;


public sealed record GetAllTagQueryResponse(
    List<TagListDto> Tags,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record TagListDto(Guid Id, string Name);