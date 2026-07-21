namespace ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;

public sealed record GetByIdCategoryQueryResponse(
    CategoryDetailDto Category
);

public sealed record CategoryDetailDto(
    Guid Id,
    string Name,
    List<SubCategoryItemDto> SubCategories
);

public sealed record SubCategoryItemDto(
    Guid Id,
    string Name,
    int ProductCount
);
