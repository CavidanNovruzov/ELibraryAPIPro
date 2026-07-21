using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.CreateOrder;

/// <summary>
/// Yeni sifariş yaratma əmri. OrderStatusId daxil edilmir —
/// handler avtomatik olaraq "Pending" statusunu tətbiq edir.
/// </summary>
public sealed record CreateOrderCommandRequest(
    string  OrderNote,
    Guid    PaymentMethodId,
    Guid    ShippingMethodId,
    Guid    UserAddressId,
    string? PromoCode = null
) : IRequest<Result<CreateOrderCommandResponse>>;
