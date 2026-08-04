using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;

public sealed class CreateWishlistCommandHandler : IRequestHandler<CreateWishlistCommandRequest, Result<CreateWishlistCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateWishlistCommandResponse>> Handle(CreateWishlistCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<CreateWishlistCommandResponse>.Failure("Sistemə daxil olunmamışdır.", ErrorType.Unauthorized);

        var wishlistReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var wishlistWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Wishlist, Guid>();

        var alreadyHasWishlist = await wishlistReadRepo.ExistsAsync(x => x.UserId == userId, tracking: false, ct: ct);
        if (alreadyHasWishlist)
            return Result<CreateWishlistCommandResponse>.Conflict("Sizin üçün istək siyahısı artıq mövcuddur.");

        var wishlist = new Domain.Entities.Concrete.Wishlist
        {
            UserId = userId
        };

        await wishlistWriteRepo.AddAsync(wishlist, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateWishlistCommandResponse>.Success(
            new CreateWishlistCommandResponse(wishlist.Id),
            "İstək siyahısı uğurla yaradıldı.");
    }
}