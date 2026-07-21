using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;

public sealed class GetProductsByCampaignQueryHandler
    : IRequestHandler<GetProductsByCampaignQueryRequest, Result<GetProductsByCampaignQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsByCampaignQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetProductsByCampaignQueryResponse>> Handle(GetProductsByCampaignQueryRequest request, CancellationToken ct)
    {
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var query = productReadRepo.GetAll(tracking: false)
            .Where(p => p.IsActive && !p.IsDeleted && p.ProductCampaigns.Any(pc => pc.CampaignId == request.CampaignId));

        int totalCount = await query.CountAsync(ct);

        var products = await query
            .OrderByDescending(p => p.CreatedDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(p => new CampaignProductDto(
                p.Id,
                p.Title,
                p.SalePrice,
                p.DiscountPrice
            ))
            .ToListAsync(ct);

        var response = new GetProductsByCampaignQueryResponse(totalCount, products);

        return Result<GetProductsByCampaignQueryResponse>.Success(response);
    }
}
