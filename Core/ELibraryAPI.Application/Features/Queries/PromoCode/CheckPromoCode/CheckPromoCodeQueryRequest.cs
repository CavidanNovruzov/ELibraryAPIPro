using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.PromoCode.CheckPromoCode;

public sealed record CheckPromoCodeQueryRequest(string Code) : IRequest<Result<CheckPromoCodeQueryResponse>>;
