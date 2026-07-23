using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.DeleteCampaign;

public sealed class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCampaignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCampaignCommandRequest request, CancellationToken ct)
    {
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Campaign, Guid>();

        var isRemoved = await writeRepo.RemoveAsync(request.Id, ct);

        if (!isRemoved)
            return Result.Failure("Kampaniya tapılmadı.");

        await _unitOfWork.SaveAsync(ct);
        return Result.Success("Kampaniya uğurla silindi.");
    }
}