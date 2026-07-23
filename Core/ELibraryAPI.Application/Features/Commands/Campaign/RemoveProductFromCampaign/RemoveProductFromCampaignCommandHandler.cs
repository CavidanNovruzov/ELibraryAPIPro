using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Campaign.RemoveProductFromCampaign;

public sealed class RemoveProductFromCampaignCommandHandler : IRequestHandler<RemoveProductFromCampaignCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProductFromCampaignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveProductFromCampaignCommandRequest request, CancellationToken ct)
    {
        var productCampaignReadRepo = _unitOfWork.ReadRepository<ProductCampaign, Guid>();
        var productCampaignWriteRepo = _unitOfWork.WriteRepository<ProductCampaign, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<ELibraryAPI.Domain.Entities.Concrete.Product, Guid>();
        var productWriteRepo = _unitOfWork.WriteRepository<ELibraryAPI.Domain.Entities.Concrete.Product, Guid>();

        var productCampaign = await productCampaignReadRepo.GetAll(tracking: true)
            .FirstOrDefaultAsync(x => x.CampaignId == request.CampaignId && x.ProductId == request.ProductId, ct);

        if (productCampaign == null) return Result.Failure("Məhsul bu kampaniyaya aid deyil.", ErrorType.NotFound);

        var product = await productReadRepo.GetByIdAsync(request.ProductId, tracking: true, ct: ct);
        if (product != null)
        {
            // Reset discount if this was the active campaign (Simplified logic)
            // In a real scenario, we would check if there are other active campaigns
            product.DiscountPrice = null;
            productWriteRepo.Update(product);
        }

        productCampaignWriteRepo.Remove(productCampaign);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Məhsul kampaniyadan uğurla silindi.");
    }
}
