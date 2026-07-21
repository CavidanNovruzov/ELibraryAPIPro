using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;

public sealed class DeleteWishlistItemCommandHandler : IRequestHandler<DeleteWishlistItemCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWishlistItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteWishlistItemCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var item = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (item == null)
            return Result.Failure("Wishlist item not found.");

        writeRepo.Remove(item);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Item permanently removed from wishlist.");
    }
}