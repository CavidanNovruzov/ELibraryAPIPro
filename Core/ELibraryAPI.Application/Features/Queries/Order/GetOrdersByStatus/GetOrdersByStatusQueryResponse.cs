using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;


namespace ELibraryAPI.Application.Features.Queries.Order.GetOrdersByStatus;

public sealed record GetOrdersByStatusQueryResponse(List<OrderListDto> Orders);
