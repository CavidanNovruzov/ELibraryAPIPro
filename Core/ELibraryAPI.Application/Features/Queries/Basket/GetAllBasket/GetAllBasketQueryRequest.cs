using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Basket.GetAllBasket;

public sealed record GetAllBasketQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllBasketQueryResponse>>;