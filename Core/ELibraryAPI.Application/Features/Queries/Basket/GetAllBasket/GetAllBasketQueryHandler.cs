using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Basket.GetAllBasket;

public sealed class GetAllBasketQueryHandler : IRequestHandler<GetAllBasketQueryRequest, Result<GetAllBasketQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBasketQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllBasketQueryResponse>> Handle(GetAllBasketQueryRequest request, CancellationToken ct)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(ct);

        var baskets = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(b => new BasketListDto(
                b.Id,
                b.UserId,
                b.User.Email ?? string.Empty,
                b.BasketItems.Sum(bi => (bi.Product.DiscountPrice ?? bi.Product.SalePrice) * bi.Quantity),
                b.BasketItems.Count
            ))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllBasketQueryResponse>.Success(
            new GetAllBasketQueryResponse(baskets, totalCount, request.Page, request.Size, totalPages));
    }
}