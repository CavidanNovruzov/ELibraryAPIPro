namespace ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;

public sealed record GetByIdProductQueryResponse(
    ProductDetailDto Product
);

public sealed record ProductDetailDto(
    Guid Id,
    string Title,
    string Description,
    string ISBN,
    int PageCount,
    int PublicationYear,
    decimal SalePrice,
    decimal? DiscountPrice,
    int StockCount,
    string PublisherName,
    string LanguageName,
    string CoverTypeName,
    string CategoryName,
    string SubCategoryName,
    List<ProductImageDto> Images,
    List<string> Authors,
    List<string> Genres,
    List<string> Tags,
    List<ReviewItemDto> Reviews,
    int ReviewCount,
    double AverageRating
);

public sealed record ProductImageDto(
    Guid Id,
    string ImageUrl,
    bool IsMain
);

public sealed record ReviewItemDto(
    Guid Id,
    string UserEmail,
    string Comment,
    int Rating,
    DateTime CreatedDate
);
