using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;

public sealed class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommandRequest, Result<CreateBannerCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public CreateBannerCommandHandler(IUnitOfWork unitOfWork, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<Result<CreateBannerCommandResponse>> Handle(CreateBannerCommandRequest request, CancellationToken ct)
    {
        byte[] fileBytes = Convert.FromBase64String(request.Base64File);

        using var fileStream = new MemoryStream(fileBytes);

        string imageUrl = await _storageService.UploadAsync(fileStream, request.FileName, "banners");

        var banner = new Domain.Entities.Concrete.Banner
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ImageUrl = imageUrl,
            Order = request.Order,
            RedirectUrl = request.RedirectUrl,
            IsActive = request.IsActive
        };

        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Banner, Guid>();
        await writeRepo.AddAsync(banner);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<CreateBannerCommandResponse>.Success(new CreateBannerCommandResponse(banner.Id));
        }

        return Result<CreateBannerCommandResponse>.Failure("Banner yaradılarkən texniki xəta baş verdi.");
    }
}