using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.UpdateCampaign;

public sealed class UpdateCampaignCommandHandler : IRequestHandler<UpdateCampaignCommandRequest, Result<UpdateCampaignCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateCampaignCommandResponse>> Handle(UpdateCampaignCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Campaign, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Campaign, Guid>();

        var campaign = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (campaign == null)
            return Result<UpdateCampaignCommandResponse>.Failure("Campaign not found.");

        if (campaign.Title.ToLower() != request.Title.Trim().ToLower())
        {
            var isTitleExists = await readRepo.ExistsAsync(
                x => x.Title.ToLower() == request.Title.Trim().ToLower() && x.Id != request.Id,
                tracking: false,
                ct: ct);

            if (isTitleExists)
                return Result<UpdateCampaignCommandResponse>.Failure("Another active campaign with this title already exists.");
        }

        _mapper.Map(request, campaign);

        writeRepo.Update(campaign);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<UpdateCampaignCommandResponse>.Success(
                new UpdateCampaignCommandResponse(campaign.Id),
                "Campaign updated successfully.");
        }

        return Result<UpdateCampaignCommandResponse>.Failure("No changes were saved.");
    }
}