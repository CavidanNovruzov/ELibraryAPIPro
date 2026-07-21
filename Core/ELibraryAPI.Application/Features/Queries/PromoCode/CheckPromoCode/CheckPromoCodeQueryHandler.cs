using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.PromoCode.CheckPromoCode;

public sealed class CheckPromoCodeQueryHandler : IRequestHandler<CheckPromoCodeQueryRequest, Result<CheckPromoCodeQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckPromoCodeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CheckPromoCodeQueryResponse>> Handle(CheckPromoCodeQueryRequest request, CancellationToken cancellationToken)
    {
        var promo = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>()
            .GetAll(tracking: false)
            .FirstOrDefaultAsync(pc => pc.Code == request.Code, cancellationToken);

        if (promo == null)
            return Result<CheckPromoCodeQueryResponse>.Failure("This promo code does not exist.", ErrorType.NotFound);

        if (!promo.IsActive)
            return Result<CheckPromoCodeQueryResponse>.Failure("This promo code is no longer active.", ErrorType.ValidationError);

        if (promo.EndDate < DateTime.UtcNow)
            return Result<CheckPromoCodeQueryResponse>.Failure("This promo code has expired.", ErrorType.ValidationError);

        if (promo.UsageCount >= promo.UsageLimit)
            return Result<CheckPromoCodeQueryResponse>.Failure("This promo code has reached its usage limit.", ErrorType.ValidationError);

        return Result<CheckPromoCodeQueryResponse>.Success(new CheckPromoCodeQueryResponse(
            promo.Code,
            promo.DiscountPercent,
            "This promo code is valid."));
    }
}