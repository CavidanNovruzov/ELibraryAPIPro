using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;

public sealed record GetProductsByCampaignQueryRequest : IRequest<Result<GetProductsByCampaignQueryResponse>>, ICacheable
{
    public Guid CampaignId { get; init; }
    public int Page { get; init; }
    public int Size { get; init; }

    public GetProductsByCampaignQueryRequest(Guid campaignId, int page = 1, int size = 20)
    {
        CampaignId = campaignId;
        Page = page < 1 ? 1 : page;
        Size = size switch
        {
            < 1 => 10,
            > 100 => 100, 
            _ => size
        };
    }

    public string CacheKey => CacheKeyHelper.Create("campaign", "products", CampaignId.ToString(), Page.ToString(), Size.ToString());
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
}
