using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.ToggleCampaignStatus;

public sealed class ToggleCampaignStatusCommandHandler : IRequestHandler<ToggleCampaignStatusCommandRequest, Result<ToggleCampaignStatusResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCampaignStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ToggleCampaignStatusResponse>> Handle(ToggleCampaignStatusCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Campaign, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Campaign, Guid>();

        var campaign = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (campaign == null)
        {
            return Result<ToggleCampaignStatusResponse>.Failure("Campaign not found.");
        }

        campaign.IsActive = !campaign.IsActive;

        writeRepo.Update(campaign);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            string status = campaign.IsActive ? "activated" : "deactivated";
            return Result<ToggleCampaignStatusResponse>.Success(
                new ToggleCampaignStatusResponse(campaign.Id, campaign.IsActive),
                $"Campaign has been {status} successfully.");
        }

        return Result<ToggleCampaignStatusResponse>.Failure("An error occurred while toggling campaign status.");
    }
}