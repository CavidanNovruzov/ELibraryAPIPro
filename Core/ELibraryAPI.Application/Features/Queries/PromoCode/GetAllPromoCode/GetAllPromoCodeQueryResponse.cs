namespace ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;

public sealed record GetAllPromoCodeQueryResponse(
    List<PromoCodeListDto> PromoCodes,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record PromoCodeListDto(
    Guid Id,
    string Code,
    decimal DiscountPercent,
    DateTime StartDate,
    DateTime EndDate,
    int UsageLimit
);