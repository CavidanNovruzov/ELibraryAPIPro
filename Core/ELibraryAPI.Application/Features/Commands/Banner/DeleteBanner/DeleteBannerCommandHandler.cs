using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.DeleteBanner;

public sealed class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteBannerCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteBannerCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Banner, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Banner, Guid>();

        var banner = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (banner is null)
        {
            return Result.Failure("Banner tapılmadı və ya artıq silinib.");
        }

        writeRepo.Remove(banner);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("banner", request.Id), ct);

            return Result.Success("Banner uğurla silindi.");
        }

        return Result.Failure("Banner silinərkən xəta baş verdi.");
    }
}