using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.Order.GetMyOrders;

public sealed record GetMyOrdersQueryRequest(int Page = 1, int Size = 10)
 : IRequest<Result<GetMyOrdersQueryResponse>>;
