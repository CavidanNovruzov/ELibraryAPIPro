using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.DeleteCampaign;

public sealed record DeleteCampaignCommandRequest(Guid Id) : IRequest<Result>;
