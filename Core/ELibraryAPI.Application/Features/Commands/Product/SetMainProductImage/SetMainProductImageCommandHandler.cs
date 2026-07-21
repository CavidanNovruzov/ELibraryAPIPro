using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Commands.Product.SetMainProductImage
{
    public sealed class SetMainProductImageCommandHandler : IRequestHandler<SetMainProductImageCommandRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public SetMainProductImageCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result> Handle(SetMainProductImageCommandRequest request, CancellationToken ct)
        {
            var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
            var imageWriteRepo = _unitOfWork.WriteRepository<ProductImage, Guid>();

            var product = await productReadRepo.GetAll(tracking: true)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

            if (product == null)
                return Result.Failure("Product not found.");

            var targetImage = product.Images.FirstOrDefault(i => i.Id == request.ImageId);
            if (targetImage == null)
                return Result.Failure("Image not found.");

            foreach (var img in product.Images)
            {
                img.IsMain = img.Id == request.ImageId;
                imageWriteRepo.Update(img);
            }

            await _unitOfWork.SaveAsync(ct);

            await _mediator.Publish(new EntityChangedEvent("product", request.ProductId), ct);

            return Result.Success("Main image updated successfully.");
        }
    }
}
