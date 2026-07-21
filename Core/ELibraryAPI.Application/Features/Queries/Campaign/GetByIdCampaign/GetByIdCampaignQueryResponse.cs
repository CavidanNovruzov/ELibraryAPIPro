namespace ELibraryAPI.Application.Features.Queries.Campaign.GetByIdCampaign;

public sealed record GetByIdCampaignQueryResponse(CampaignDetailDto Campaign);

public sealed record CampaignDetailDto(
    Guid Id,
    string Title,
    string Description,
    decimal DiscountPercent,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    bool IsExpired 
);