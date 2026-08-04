using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Abstractions.Services;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.DeleteWishlist;

public sealed class DeleteWishlistCommandHandler : IRequestHandler<DeleteWishlistCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteWishlistCommandRequest request, CancellationToken ct)
    {
        var wishlistReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var wishlistWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Wishlist, Guid>();

        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var wishlist = await wishlistReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (wishlist == null)
            return Result.NotFound("İstək siyahısı tapılmadı.");

        // Ownership check: only owner or admin can delete
        if (wishlist.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu resursa müdaxilə etmək üçün icazəniz yoxdur.");

        wishlistWriteRepo.Remove(wishlist);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("İstək siyahısı uğurla arxivləşdirildi.");
    }
}
