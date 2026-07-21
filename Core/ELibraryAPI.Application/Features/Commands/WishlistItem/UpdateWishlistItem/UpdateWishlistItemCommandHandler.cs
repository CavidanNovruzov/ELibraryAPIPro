using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;

public sealed class UpdateWishlistItemCommandHandler : IRequestHandler<UpdateWishlistItemCommandRequest, Result<UpdateWishlistItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWishlistItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateWishlistItemCommandResponse>> Handle(UpdateWishlistItemCommandRequest request, CancellationToken ct)
    {
        var itemRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var wishlistRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var productRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var item = await itemRead.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (item == null)
            return Result<UpdateWishlistItemCommandResponse>.Failure("Wishlist item not found.");

        if (!await wishlistRead.ExistsAsync(x => x.Id == request.WishlistId, false, ct))
            return Result<UpdateWishlistItemCommandResponse>.Failure("Target wishlist not found.");

        if (!await productRead.ExistsAsync(x => x.Id == request.ProductId, false, ct))
            return Result<UpdateWishlistItemCommandResponse>.Failure("Target product not found.");

        var duplicate = await itemRead.ExistsAsync(
            x => x.Id != request.Id && x.WishlistId == request.WishlistId && x.ProductId == request.ProductId,
            false, ct);

        if (duplicate)
            return Result<UpdateWishlistItemCommandResponse>.Failure("This product is already in the target wishlist.");

        item.WishlistId = request.WishlistId;
        item.ProductId = request.ProductId;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateWishlistItemCommandResponse>.Success(
            new UpdateWishlistItemCommandResponse(item.Id),
            "Wishlist item updated successfully.");
    }
}