using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;

public sealed class CreateWishlistItemCommandHandler : IRequestHandler<CreateWishlistItemCommandRequest, Result<CreateWishlistItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWishlistItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateWishlistItemCommandResponse>> Handle(CreateWishlistItemCommandRequest request, CancellationToken ct)
    {
        var wishlistRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var productRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var itemRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var itemWrite = _unitOfWork.WriteRepository<Domain.Entities.Concrete.       WishlistItem, Guid>();

        if (!await wishlistRead.ExistsAsync(x => x.Id == request.WishlistId, false, ct))
            return Result<CreateWishlistItemCommandResponse>.Failure("Wishlist not found.");

        if (!await productRead.ExistsAsync(x => x.Id == request.ProductId, false, ct))
            return Result<CreateWishlistItemCommandResponse>.Failure("Product not found.");

        var alreadyExists = await itemRead.ExistsAsync(
            x => x.WishlistId == request.WishlistId && x.ProductId == request.ProductId, false, ct);

        if (alreadyExists)
            return Result<CreateWishlistItemCommandResponse>.Failure("Product already exists in wishlist.");

        var item = new Domain.Entities.Concrete.WishlistItem
        {
            WishlistId = request.WishlistId,
            ProductId = request.ProductId
        };

        await itemWrite.AddAsync(item, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateWishlistItemCommandResponse>.Success(
            new CreateWishlistItemCommandResponse(item.Id),
            "Item added to wishlist successfully.");
    }
}