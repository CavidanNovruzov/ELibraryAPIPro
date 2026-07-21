using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;

public sealed record UpdatePromoCodeCommandRequest(
    Guid Id,
    string Code,
    decimal DiscountPercent,
    DateTime EndDate,
    DateTime StartDate,
    int UsageLimit
) : IRequest<Result<UpdatePromoCodeCommandResponse>>;
