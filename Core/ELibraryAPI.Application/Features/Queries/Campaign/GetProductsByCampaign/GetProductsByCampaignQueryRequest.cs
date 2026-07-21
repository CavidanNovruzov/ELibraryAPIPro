using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;

public sealed record GetProductsByCampaignQueryRequest(Guid CampaignId, int Page = 1, int Size = 20)
 : IRequest<Result<GetProductsByCampaignQueryResponse>>, ICacheable
{
    public string CacheKey => CacheKeyHelper.Create("campaign", "products", CampaignId.ToString(), Page.ToString(), Size.ToString());

    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
}
