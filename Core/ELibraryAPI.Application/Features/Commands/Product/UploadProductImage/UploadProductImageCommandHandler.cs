using ELibraryAPI.Application.Abstractions.Services.Image;
using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.UploadProductImage;

public sealed class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, Result<UploadProductImageCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;

    public UploadProductImageCommandHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        IImageService imageService,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _imageService = imageService;
        _mediator = mediator;
    }

    public async Task<Result<UploadProductImageCommandResponse>> Handle(UploadProductImageCommandRequest request, CancellationToken ct)
    {
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var product = await productReadRepo.GetByIdAsync(
            id: request.ProductId,
            tracking: true,
            ct: ct,
            includes: p => p.Images
        );

        if (product == null)
            return Result<UploadProductImageCommandResponse>.Failure("Məhsul tapılmadı..");

        var urls = new List<string>();

        foreach (var file in request.Files)
        {
            using var processedImageStream = await _imageService.CompressAndConvertToWebpAsync(
                inputStream: file.Content,
                maxWidth: 1200,
                maxHeight: 1200,
                quality: 80,
                ct: ct);

            var uniqueFileName = $"{Guid.NewGuid()}.webp";

            var pathOrUrl = await _storageService.UploadAsync(processedImageStream, uniqueFileName, "product-images");
            urls.Add(pathOrUrl);

            product.Images.Add(new ProductImage
            {
                Id = Guid.NewGuid(),
                ImageUrl = pathOrUrl,
                IsMain = false,
                ProductId = product.Id
            });
        }

        _unitOfWork.WriteRepository<Domain.Entities.Concrete.Product, Guid>().Update(product);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("product", request.ProductId), ct);

        return Result<UploadProductImageCommandResponse>.Success(new UploadProductImageCommandResponse(urls));
    }
}