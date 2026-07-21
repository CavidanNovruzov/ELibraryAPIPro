using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetByIdCampaign;

public sealed record GetByIdCampaignQueryRequest(Guid Id) : IRequest<Result<GetByIdCampaignQueryResponse>>;
