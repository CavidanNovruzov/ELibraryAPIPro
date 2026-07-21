using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.DeleteBanner;

public sealed class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBannerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBannerCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Banner, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Banner, Guid>();

        var banner = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (banner is null)
        {
            return Result.Failure("Banner not found or already archived.");
        }

        writeRepo.Remove(banner);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result.Success();

        return Result.Failure("An error occurred while archiving the banner.");
    }
}