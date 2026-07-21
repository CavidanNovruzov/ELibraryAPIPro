using System;
using System.Collections.Generic;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetAllCampaign;

public sealed record GetAllCampaignQueryResponse(
    List<CampaignListDto> Campaigns,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record CampaignListDto(
    Guid Id,
    string Title,
    string Description,
    decimal DiscountPercent,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActiveStatus
);