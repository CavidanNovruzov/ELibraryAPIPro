using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Product.GetNewArrivals;

public sealed class GetNewArrivalsQueryHandler : IRequestHandler<GetNewArrivalsQueryRequest, Result<List<GetNewArrivalsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNewArrivalsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetNewArrivalsQueryResponse>>> Handle(GetNewArrivalsQueryRequest request, CancellationToken ct)
    {
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var targetDate = DateTime.UtcNow.AddDays(-30);

        var products = await productReadRepo.GetAll(tracking: false)
            .Where(p => p.IsActive && p.CreatedDate >= targetDate)
            .OrderByDescending(p => p.CreatedDate)
            .Take(8)
            .Select(p => new GetNewArrivalsQueryResponse(
                p.Id,
                p.Title,
                p.SalePrice,
                p.DiscountPrice,
                p.Images.Where(img => img.IsMain).Select(img => img.ImageUrl).FirstOrDefault(),
                p.CreatedDate
            ))
            .ToListAsync(ct);

        return Result<List<GetNewArrivalsQueryResponse>>.Success(products);
    }
}