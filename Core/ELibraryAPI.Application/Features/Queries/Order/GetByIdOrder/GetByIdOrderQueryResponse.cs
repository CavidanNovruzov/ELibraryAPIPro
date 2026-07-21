namespace ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;

public sealed record GetByIdOrderQueryResponse(
    OrderDetailDto Order
);

public sealed record OrderDetailDto(
    Guid Id,
    string OrderNumber,
    DateTime CreatedDate,
    decimal TotalAmount,
    string OrderNote,
    string OrderStatusName,
    string PaymentMethodName,
    string ShippingMethodName,
    string UserEmail,
    string UserPhone,
    List<OrderItemDetailDto> Items
);

public sealed record OrderItemDetailDto(
    Guid Id,
    Guid ProductId,
    string ProductTitle,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);
