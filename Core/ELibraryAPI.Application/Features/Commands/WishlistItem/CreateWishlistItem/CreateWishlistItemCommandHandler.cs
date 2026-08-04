using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;

public sealed class CreateWishlistItemCommandHandler : IRequestHandler<CreateWishlistItemCommandRequest, Result<CreateWishlistItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateWishlistItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateWishlistItemCommandResponse>> Handle(CreateWishlistItemCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<CreateWishlistItemCommandResponse>.Failure("Sistemə daxil olunmamışdır.", ErrorType.Unauthorized);

        var wishlistRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var productRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var itemRead = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var itemWrite = _unitOfWork.WriteRepository<Domain.Entities.Concrete.WishlistItem, Guid>();

        if (!await productRead.ExistsAsync(x => x.Id == request.ProductId, false, ct))
            return Result<CreateWishlistItemCommandResponse>.NotFound("Məhsul tapılmadı.");

        var wishlist = await wishlistRead.GetSingleAsync(x => x.UserId == userId, tracking: false, ct: ct);
        if (wishlist == null)
            return Result<CreateWishlistItemCommandResponse>.NotFound("İstək siyahınız tapılmadı. Zəhmət olmasa əvvəlcə istək siyahısı yaradın.");

        var alreadyExists = await itemRead.ExistsAsync(
            x => x.WishlistId == wishlist.Id && x.ProductId == request.ProductId, false, ct);

        if (alreadyExists)
            return Result<CreateWishlistItemCommandResponse>.Conflict("Məhsul artıq istək siyahısındadır.");

        var item = new Domain.Entities.Concrete.WishlistItem
        {
            WishlistId = wishlist.Id,
            ProductId = request.ProductId
        };

        await itemWrite.AddAsync(item, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateWishlistItemCommandResponse>.Success(
            new CreateWishlistItemCommandResponse(item.Id),
            "Məhsul istək siyahısına əlavə edildi.");
    }
}