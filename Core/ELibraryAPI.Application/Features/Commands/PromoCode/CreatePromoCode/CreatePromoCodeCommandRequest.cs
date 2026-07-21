using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;

public sealed record CreatePromoCodeCommandRequest(
    string Code,
    decimal DiscountPercent,
    DateTime EndDate,
    DateTime StartDate,
    int UsageLimit
) : IRequest<Result<CreatePromoCodeCommandResponse>>;
