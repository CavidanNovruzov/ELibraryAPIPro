using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.RemoveProductFromCampaign;

public sealed record RemoveProductFromCampaignCommandRequest(
    Guid CampaignId,
    Guid ProductId
) : IRequest<Result>;
