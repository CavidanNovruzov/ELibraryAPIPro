

namespace ELibraryAPI.Application.Features.Queries.Product.GetFeaturedProducts;

public sealed record GetFeaturedProductsQueryResponse(
 Guid Id,
 string Title,
 string ISBN,
 decimal SalePrice,
 decimal? DiscountPrice,
 string? MainImageUrl,
 double Rating);
