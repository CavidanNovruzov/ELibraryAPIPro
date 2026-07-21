

namespace ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;

public sealed record GetAllOrderQueryResponse(
    List<OrderListDto> Orders,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record OrderListDto(
    Guid Id,
    string OrderNumber,
    DateTime CreatedDate,
    decimal TotalAmount,
    string OrderStatusName,
    string UserEmail,
    int ItemCount
);