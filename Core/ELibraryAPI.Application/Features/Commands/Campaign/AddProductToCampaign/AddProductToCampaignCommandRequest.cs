using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.AddProductToCampaign;

public sealed record AddProductToCampaignCommandRequest(
    Guid CampaignId,
    Guid ProductId
) : IRequest<Result>;
