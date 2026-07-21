namespace ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;

public sealed record GetAllOrderStatusQueryResponse(
    List<OrderStatusListDto> OrderStatuses,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record OrderStatusListDto(Guid Id, string Name);