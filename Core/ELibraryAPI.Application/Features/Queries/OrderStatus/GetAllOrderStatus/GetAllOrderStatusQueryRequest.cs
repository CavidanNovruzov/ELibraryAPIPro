using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;

public sealed record GetAllOrderStatusQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllOrderStatusQueryResponse>>;
