

namespace ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch;

public sealed record GetCategorySearchQueryResponse(
    List<CategorySearchDto> Results,
    int TotalCount);

    public sealed record CategorySearchDto(
    Guid Id,
    string Name,
    string Type 
);