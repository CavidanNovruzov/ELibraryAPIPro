using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.ToggleCampaignStatus;

public sealed record ToggleCampaignStatusCommandRequest(Guid Id) : IRequest<Result<ToggleCampaignStatusResponse>>;