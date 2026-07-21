namespace ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;

public sealed record GetAllProductQueryResponse(
    List<ProductListDto> Products,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record ProductListDto(
    Guid Id,
    string Title,
    string ISBN,
    decimal SalePrice,
    decimal? DiscountPrice,
    int PageCount,
    string PublisherName,
    string LanguageName,
    string CoverTypeName,
    string CategoryName,
    List<string> Authors,
    List<string> Genres,
    int ReviewCount,
    double AverageRating,
    string ImageUrl
);
