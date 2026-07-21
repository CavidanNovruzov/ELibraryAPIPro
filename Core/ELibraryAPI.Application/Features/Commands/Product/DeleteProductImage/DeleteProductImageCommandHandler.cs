using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Product.DeleteProductImage
{
    public sealed class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IMediator _mediator;

        public DeleteProductImageCommandHandler(IUnitOfWork unitOfWork, IStorageService storageService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _mediator = mediator;
        }

        public async Task<Result> Handle(DeleteProductImageCommandRequest request, CancellationToken ct)
        {
            var imageReadRepo = _unitOfWork.ReadRepository<ProductImage, Guid>();
            var imageWriteRepo = _unitOfWork.WriteRepository<ProductImage, Guid>();

            var image = await imageReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
            if (image == null)
                return Result.Failure("Image not found.");

            var fileName = Path.GetFileName(image.ImageUrl);
            _storageService.Delete("product-images", fileName);

            imageWriteRepo.Remove(image);

            await _unitOfWork.SaveAsync(ct);

            await _mediator.Publish(new EntityChangedEvent("product", image.ProductId), ct);

            return Result.Success("Image deleted successfully.");
        }
    }
}
