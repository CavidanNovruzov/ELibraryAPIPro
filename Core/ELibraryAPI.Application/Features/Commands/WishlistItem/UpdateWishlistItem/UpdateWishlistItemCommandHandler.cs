using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;

public sealed class UpdateWishlistItemCommandHandler : IRequestHandler<UpdateWishlistItemCommandRequest, Result<UpdateWishlistItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateWishlistItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UpdateWishlistItemCommandResponse>> Handle(UpdateWishlistItemCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<UpdateWishlistItemCommandResponse>.Failure("Sistemdə daxil edilməmisiniz.", ErrorType.Unauthorized);

        var itemRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var wishlistRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var productRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var item = await itemRead.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (item == null)
            return Result<UpdateWishlistItemCommandResponse>.NotFound("İstək siyahısı elementi tapılmadı.");

        var sourceWishlist = await wishlistRead.GetByIdAsync(item.WishlistId, tracking: false, ct: ct);
        if (sourceWishlist == null)
            return Result<UpdateWishlistItemCommandResponse>.NotFound("Mənbə istək siyahısı tapılmadı.");

        if (sourceWishlist.UserId != userId && !_currentUserService.IsAdmin)
            return Result<UpdateWishlistItemCommandResponse>.Forbidden("Bu əməliyyatı yerinə yetirmək üçün icazəniz yoxdur.");

        if (!await wishlistRead.ExistsAsync(x => x.Id == request.WishlistId, false, ct))
            return Result<UpdateWishlistItemCommandResponse>.NotFound("Hədəf istək siyahısı tapılmadı.");

        var targetWishlist = await wishlistRead.GetByIdAsync(request.WishlistId, tracking: false, ct: ct);
        if (targetWishlist == null)
            return Result<UpdateWishlistItemCommandResponse>.NotFound("Hədəf istək siyahısı tapılmadı.");

        if (targetWishlist.UserId != userId && !_currentUserService.IsAdmin)
            return Result<UpdateWishlistItemCommandResponse>.Forbidden("Hədəf istək siyahısına məhsul əlavə etmək üçün icazəniz yoxdur.");

        if (!await productRead.ExistsAsync(x => x.Id == request.ProductId, false, ct))
            return Result<UpdateWishlistItemCommandResponse>.NotFound("Hədəf məhsul tapılmadı.");

        var duplicate = await itemRead.ExistsAsync(
            x => x.Id != request.Id && x.WishlistId == request.WishlistId && x.ProductId == request.ProductId,
            false, ct);

        if (duplicate)
            return Result<UpdateWishlistItemCommandResponse>.Conflict("Bu məhsul artıq hədəf istək siyahısındadır.");

        item.WishlistId = request.WishlistId;
        item.ProductId = request.ProductId;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateWishlistItemCommandResponse>.Success(
            new UpdateWishlistItemCommandResponse(item.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}
