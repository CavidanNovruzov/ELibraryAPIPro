using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetAllCampaign;

public sealed record GetAllCampaignQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllCampaignQueryResponse>>;