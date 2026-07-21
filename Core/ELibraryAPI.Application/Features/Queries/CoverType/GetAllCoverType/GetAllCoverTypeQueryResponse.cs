namespace ELibraryAPI.Application.Features.Queries.CoverType.GetAllCoverType;

public sealed record GetAllCoverTypeQueryResponse(
    List<CoverTypeListDto> CoverTypes,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record CoverTypeListDto(Guid Id, string Name, int ProductCount);
