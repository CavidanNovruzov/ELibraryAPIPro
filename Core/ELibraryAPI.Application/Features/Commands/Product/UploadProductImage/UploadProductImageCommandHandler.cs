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
    private readonly IMediator _mediator; 

    public UploadProductImageCommandHandler(IUnitOfWork unitOfWork, IStorageService storageService, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
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
            return Result<UploadProductImageCommandResponse>.Failure("Product not found.");

        var urls = new List<string>();

        foreach (var file in request.Files)
        {
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var pathOrUrl = await _storageService.UploadAsync(file.Content, uniqueFileName, "product-images");
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