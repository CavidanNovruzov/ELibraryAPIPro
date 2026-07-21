using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;

public sealed record UpdateOrderCommandRequest(
    Guid Id,
    string OrderNote,
    string OrderNumber,
    Guid OrderStatusId,
    Guid PaymentMethodId,
    Guid ShippingMethodId,
    decimal TotalAmount,
    Guid UserId
) : IRequest<Result<UpdateOrderCommandResponse>>;
