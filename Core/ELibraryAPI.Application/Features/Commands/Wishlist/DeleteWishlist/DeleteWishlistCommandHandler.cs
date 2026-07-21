using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.DeleteWishlist;

public sealed class DeleteWishlistCommandHandler : IRequestHandler<DeleteWishlistCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWishlistCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteWishlistCommandRequest request, CancellationToken ct)
    {
        var wishlistReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var wishlistWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Wishlist, Guid>();

        var wishlist = await wishlistReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (wishlist == null)
            return Result.Failure("Wishlist not found.");

        wishlistWriteRepo.Remove(wishlist);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Wishlist moved to archive.");
    }
}
