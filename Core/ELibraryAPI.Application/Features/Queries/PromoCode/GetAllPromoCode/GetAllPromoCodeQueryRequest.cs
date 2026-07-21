using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;

public sealed record GetAllPromoCodeQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllPromoCodeQueryResponse>>;
