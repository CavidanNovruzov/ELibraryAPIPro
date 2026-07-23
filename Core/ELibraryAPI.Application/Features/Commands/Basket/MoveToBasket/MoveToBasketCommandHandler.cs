using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;

public sealed class MoveToBasketCommandHandler : IRequestHandler<MoveToBasketCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public MoveToBasketCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(MoveToBasketCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Failure("Sistemə daxil olmuş istifadəçi tapılmadı.", ErrorType.Unauthorized);

        var basket = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetSingleAsync(b => b.UserId == userId, tracking: true, ct: ct);

        if (basket == null)
            return Result.Failure("Cari istifadəçi üçün səbət tapılmadı.", ErrorType.NotFound);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var wishlistItem = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>()
                .GetByIdAsync(request.WishlistItemId, tracking: true, ct: ct);

            if (wishlistItem == null) return Result.Failure("İstək siyahısı elementi tapılmadı.", ErrorType.NotFound);

            var product = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>()
                .GetAll()
                .Include(p => p.Stocks)
                .FirstOrDefaultAsync(p => p.Id == wishlistItem.ProductId, ct);

            if (product == null)
                return Result.Failure("Məhsul tapılmadı..", ErrorType.NotFound);

            if (product.Stocks.Sum(s => s.Quantity) <= 0)
                return Result.Failure("Məhsul stokda yoxdur.", ErrorType.ValidationError);

            var basketItem = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>()
                .GetSingleAsync(b => b.BasketId == basket.Id && b.ProductId == wishlistItem.ProductId, tracking: true, ct: ct);

            if (basketItem != null)
            {
                basketItem.Quantity += 1;
            }
            else
            {
                await _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>().AddAsync(new Domain.Entities.Concrete.BasketItem
                {
                    BasketId = basket.Id,
                    ProductId = wishlistItem.ProductId,
                    Quantity = 1
                }, ct);
            }

            _unitOfWork.WriteRepository<Domain.Entities.Concrete.WishlistItem, Guid>().Remove(wishlistItem);

            await _unitOfWork.SaveAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success("Səbətə uğurla köçürüldü.");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure("Məhsul səbətə köçürülərkən xəta baş verdi.");
        }
    }
}
