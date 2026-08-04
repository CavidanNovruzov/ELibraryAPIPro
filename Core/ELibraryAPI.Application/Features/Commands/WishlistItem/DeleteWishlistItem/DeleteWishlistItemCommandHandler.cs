using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;

public sealed class DeleteWishlistItemCommandHandler : IRequestHandler<DeleteWishlistItemCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteWishlistItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteWishlistItemCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.WishlistItem, Guid>();
        var item = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (item == null)
            return Result.NotFound("İstək siyahısı elementi tapılmadı.");

        var wishlist = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>()
            .GetByIdAsync(item.WishlistId, tracking: false, ct: ct);

        if (wishlist == null)
            return Result.NotFound("İstək siyahısı tapılmadı.");

        if (wishlist.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu elementi silmək üçün icazəniz yoxdur.");

        writeRepo.Remove(item);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Element istək siyahısından tamamilə silindi.");
    }
}
