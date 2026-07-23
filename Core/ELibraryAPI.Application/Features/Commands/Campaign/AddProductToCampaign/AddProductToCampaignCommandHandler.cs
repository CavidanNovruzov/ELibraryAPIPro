using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Campaign.AddProductToCampaign;

public sealed class AddProductToCampaignCommandHandler : IRequestHandler<AddProductToCampaignCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddProductToCampaignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddProductToCampaignCommandRequest request, CancellationToken ct)
    {
        var campaignReadRepo = _unitOfWork.ReadRepository<ELibraryAPI.Domain.Entities.Concrete.Campaign, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<ELibraryAPI.Domain.Entities.Concrete.Product, Guid>();
        var productCampaignWriteRepo = _unitOfWork.WriteRepository<ProductCampaign, Guid>();
        var productWriteRepo = _unitOfWork.WriteRepository<ELibraryAPI.Domain.Entities.Concrete.Product, Guid>();

        var campaign = await campaignReadRepo.GetByIdAsync(request.CampaignId, tracking: false, ct: ct);
        if (campaign == null) return Result.Failure("Kampaniya tapılmadı.", ErrorType.NotFound);

        var product = await productReadRepo.GetByIdAsync(request.ProductId, tracking: true, ct: ct);
        if (product == null) return Result.Failure("Məhsul tapılmadı..", ErrorType.NotFound);

        var existing = await _unitOfWork.ReadRepository<ProductCampaign, Guid>().ExistsAsync(
            x => x.CampaignId == request.CampaignId && x.ProductId == request.ProductId ,
            tracking: false, ct: ct);

        if (existing) return Result.Failure("Məhsul artıq bu kampaniyadadır.");

        var productCampaign = new ProductCampaign
        {
            CampaignId = request.CampaignId,
            ProductId = request.ProductId
        };

        // Apply discount to product
        product.DiscountPrice = product.SalePrice - (product.SalePrice * (campaign.DiscountPercent / 100));

        await productCampaignWriteRepo.AddAsync(productCampaign, ct);
        productWriteRepo.Update(product);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Məhsul kampaniyaya uğurla əlavə edildi.");
    }
}
