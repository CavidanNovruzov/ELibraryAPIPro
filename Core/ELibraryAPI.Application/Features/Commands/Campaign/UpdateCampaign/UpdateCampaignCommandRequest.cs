using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.UpdateCampaign;

public sealed record UpdateCampaignCommandRequest(
    Guid Id,
    string Title,
    string Description,
    decimal DiscountPercent,
    DateTime StartDate, 
    DateTime EndDate    
) : IRequest<Result<UpdateCampaignCommandResponse>>;

