

namespace ELibraryAPI.Application.Features.Queries.Order.GetMyOrders;

public sealed record GetMyOrdersQueryResponse(
 List<MyOrderDto> Orders,
 int TotalCount,
 int CurrentPage,
 int PageSize,
 int TotalPages);

public sealed record MyOrderDto(
    Guid Id,
    string OrderNumber,
    decimal TotalAmount,
    string OrderStatus,
    DateTime CreatedDate,
    int TotalItems);
