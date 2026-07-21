namespace ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;

public sealed record GetByIdCoverTypeQueryResponse(
    CoverTypeDetailDto CoverType
);

public sealed record CoverTypeDetailDto(
    Guid Id,
    string Name,
    int ProductCount
);
