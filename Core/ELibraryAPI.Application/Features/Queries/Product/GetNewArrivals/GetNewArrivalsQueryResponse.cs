

namespace ELibraryAPI.Application.Features.Queries.Product.GetNewArrivals;

public sealed record GetNewArrivalsQueryResponse(
    Guid Id,
    string Title,
    decimal SalePrice,
    decimal? DiscountPrice,
    string? MainImageUrl,
    DateTime CreatedDate);
