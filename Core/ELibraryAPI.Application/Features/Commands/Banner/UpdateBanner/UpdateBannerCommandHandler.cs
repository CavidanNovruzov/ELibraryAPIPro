
using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.UpdateBanner;

public sealed class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommandRequest, Result<UpdateBannerCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public UpdateBannerCommandHandler(IUnitOfWork unitOfWork, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<Result<UpdateBannerCommandResponse>> Handle(UpdateBannerCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Banner, Guid>();
        var banner = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (banner == null)
            return Result<UpdateBannerCommandResponse>.Failure("Banner not found.");


        if (!string.IsNullOrEmpty(request.Base64File) && !string.IsNullOrEmpty(request.FileName))
        {
            _storageService.Delete("banners", Path.GetFileName(banner.ImageUrl));

            byte[] fileBytes = Convert.FromBase64String(request.Base64File);

            using var fileStream = new MemoryStream(fileBytes);

            banner.ImageUrl = await _storageService.UploadAsync(fileStream, request.FileName, "banners");
        }

        banner.Title = request.Title;
        banner.RedirectUrl = request.RedirectUrl;
        banner.Order = request.Order;
        banner.IsActive = request.IsActive;

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<UpdateBannerCommandResponse>.Success(new UpdateBannerCommandResponse(banner.Id));

        return Result<UpdateBannerCommandResponse>.Failure("Update zamanı xəta baş verdi.");
    }
}