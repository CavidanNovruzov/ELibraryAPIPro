

namespace ELibraryAPI.Application.Features.Queries.PromoCode.CheckPromoCode
{
    public sealed record CheckPromoCodeQueryResponse(
        string Code,
        decimal DiscountPercent,
        string Message
    );
}
