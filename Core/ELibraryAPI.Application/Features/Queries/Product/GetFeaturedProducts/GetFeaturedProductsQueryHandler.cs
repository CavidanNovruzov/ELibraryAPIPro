using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Product.GetFeaturedProducts;

public sealed class GetFeaturedProductsQueryHandler : IRequestHandler<GetFeaturedProductsQueryRequest, Result<List<GetFeaturedProductsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeaturedProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetFeaturedProductsQueryResponse>>> Handle(GetFeaturedProductsQueryRequest request, CancellationToken ct)
    {
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var products = await productReadRepo.GetAll(tracking: false)
            .Where(p => p.IsActive && p.DiscountPrice.HasValue)
            .OrderByDescending(p => p.Reviews.Average(r => (double?)r.Rating) ?? 0)
            .Take(8)
            .Select(p => new GetFeaturedProductsQueryResponse(
                p.Id,
                p.Title,
                p.ISBN,
                p.SalePrice,
                p.DiscountPrice,
                p.Images.Where(img => img.IsMain).Select(img => img.ImageUrl).FirstOrDefault(),
                p.Reviews.Average(r => (double?)r.Rating) ?? 0
            ))
            .ToListAsync(ct);

        return Result<List<GetFeaturedProductsQueryResponse>>.Success(products);
    }
}
