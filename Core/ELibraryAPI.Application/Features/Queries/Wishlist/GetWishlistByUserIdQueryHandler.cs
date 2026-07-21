using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Wishlist.GetCustomerWishlist;

public sealed class GetCustomerWishlistQueryHandler : IRequestHandler<GetCustomerWishlistQueryRequest, Result<GetCustomerWishlistQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetCustomerWishlistQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetCustomerWishlistQueryResponse>> Handle(GetCustomerWishlistQueryRequest request, CancellationToken cancellationToken)
    {
        Guid currentUserId = Guid.Parse(_currentUserService.UserId.ToString());

        var wishlist = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>()
            .GetAll(tracking: false)
            .Where(w => w.UserId == currentUserId)
            .Select(w => new GetCustomerWishlistQueryResponse(
                w.Id,
                w.WishlistItems.Select(wi => new WishlistItemDto(
                    wi.ProductId,
                    wi.Product.Title,
                    wi.Product.ProductAuthors.Select(pa => pa.Author.FullName).FirstOrDefault() ?? "",
                    wi.Product.SalePrice,
                    wi.Product.DiscountPrice,
                    wi.Product.Images.Where(i => i.IsMain).Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                    wi.Product.Stocks.Sum(s => s.Quantity) > 0
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (wishlist == null)
        {
            return Result<GetCustomerWishlistQueryResponse>.Success(
                new GetCustomerWishlistQueryResponse(Guid.Empty, new()));
        }

        return Result<GetCustomerWishlistQueryResponse>.Success(wishlist);
    }
}