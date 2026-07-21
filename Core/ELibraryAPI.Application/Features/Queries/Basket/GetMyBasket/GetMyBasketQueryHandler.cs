using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Basket.GetMyBasket;

public sealed class GetMyBasketQueryHandler
    : IRequestHandler<GetMyBasketQueryRequest, Result<GetMyBasketQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetMyBasketQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetMyBasketQueryResponse>> Handle(
        GetMyBasketQueryRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<GetMyBasketQueryResponse>.Failure("User is not authenticated.");

        var basket = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetAll(tracking: false)
            .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket == null)
            return Result<GetMyBasketQueryResponse>.Failure("Basket not found.");

        var items = basket.BasketItems.Select(bi => new BasketItemDto(
            bi.Id,
            bi.ProductId,
            bi.Product.Title,
            bi.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? "",
            bi.Product.DiscountPrice ?? bi.Product.SalePrice,
            bi.Quantity,
            (bi.Product.DiscountPrice ?? bi.Product.SalePrice) * bi.Quantity
        )).ToList();

        var total = items.Sum(i => i.SubTotal);

        return Result<GetMyBasketQueryResponse>.Success(
            new GetMyBasketQueryResponse(basket.Id, items, total));
    }
}
