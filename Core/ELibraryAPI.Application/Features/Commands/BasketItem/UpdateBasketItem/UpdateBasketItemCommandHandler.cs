using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItem;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItemQuantity;

public sealed class UpdateBasketItemQuantityHandler : IRequestHandler<UpdateBasketItemQuantityRequest, Result<UpdateBasketItemQuantityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBasketItemQuantityHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UpdateBasketItemQuantityResponse>> Handle(UpdateBasketItemQuantityRequest request, CancellationToken ct)
    {
        var basketItemReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var basketItemWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var basketItem = await basketItemReadRepo.GetAll(tracking: true)
            .Include(bi => bi.Basket)
            .FirstOrDefaultAsync(bi => bi.Id == request.Id, ct);

        if (basketItem == null)
            return Result<UpdateBasketItemQuantityResponse>.NotFound("Səbət elementi tapılmadı.");

        if (basketItem.Basket.UserId != _currentUserService.UserGuid && !_currentUserService.IsAdmin)
            return Result<UpdateBasketItemQuantityResponse>.Forbidden("Bu səbət elementini dəyişmək üçün icazəniz yoxdur.");

        var productInfo = await productReadRepo.GetAll(tracking: false)
            .Where(p => p.Id == basketItem.ProductId)
            .Select(p => new
            {
                TotalStock = p.Stocks.Sum(s => s.Quantity)
            })
            .FirstOrDefaultAsync(ct);

        if (productInfo == null)
            return Result<UpdateBasketItemQuantityResponse>.NotFound("Əlaqədar məhsul tapılmadı.");

        if (productInfo.TotalStock < request.Quantity)
            return Result<UpdateBasketItemQuantityResponse>.Failure($"Kifayət qədər stok yoxdur. Mövcud stok: {productInfo.TotalStock}");

        basketItem.Quantity = request.Quantity;
        basketItemWriteRepo.Update(basketItem);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<UpdateBasketItemQuantityResponse>.Success(new UpdateBasketItemQuantityResponse(basketItem.Id));

        return Result<UpdateBasketItemQuantityResponse>.Failure("Miqdar yenilənərkən xəta baş verdi.");
    }
}