using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;

public sealed class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommandRequest, Result<CreateBannerCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IMediator _mediator;

    public CreateBannerCommandHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _mediator = mediator;
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
        await writeRepo.AddAsync(banner, ct);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("banner", banner.Id), ct);

            return Result<CreateBannerCommandResponse>.Success(
                new CreateBannerCommandResponse(banner.Id),
                "Banner uğurla yaradıldı.");
        }

        return Result<CreateBannerCommandResponse>.Failure("Banner yaradılarkən texniki xəta baş verdi.");
    }
}