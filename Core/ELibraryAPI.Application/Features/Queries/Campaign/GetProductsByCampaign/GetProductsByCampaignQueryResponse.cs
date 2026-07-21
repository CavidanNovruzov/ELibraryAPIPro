

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;

public sealed record GetProductsByCampaignQueryResponse(
 int TotalCount,
 List<CampaignProductDto> Products);

public sealed record CampaignProductDto(
    Guid Id,
    string Title,
    decimal Price,
    decimal? DiscountPrice);
