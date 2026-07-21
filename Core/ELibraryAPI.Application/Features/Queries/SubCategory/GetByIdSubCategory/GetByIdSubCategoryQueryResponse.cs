namespace ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;

public sealed record GetByIdSubCategoryQueryResponse(
    SubCategoryDetailDto SubCategory
);

public sealed record SubCategoryDetailDto(
    Guid Id,
    string Name,
    Guid CategoryId,
    string CategoryName,
    int ProductCount
);
