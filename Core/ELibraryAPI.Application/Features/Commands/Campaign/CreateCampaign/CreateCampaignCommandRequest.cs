using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.CreateCampaign;

public sealed record CreateCampaignCommandRequest(
    string Title,
    string Description,
    decimal DiscountPercent,
    DateTime StartDate, 
    DateTime EndDate    
) : IRequest<Result<CreateCampaignCommandResponse>>;