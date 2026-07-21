

namespace ELibraryAPI.Application.Features.Queries.Order.GetOrderByNumber;

public sealed record GetOrderByNumberQueryResponse(
    Guid Id,
    string OrderNumber,
    decimal TotalAmount,       
    string StatusName,      
    string? OrderNote,      
    DateTime CreatedDate,
    List<OrderItemDto> OrderItems);

public sealed record OrderItemDto(
    Guid ProductId,
    string ProductTitle,
    int Quantity,
    decimal Price);