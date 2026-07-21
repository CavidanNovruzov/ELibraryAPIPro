using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItem;

public sealed record UpdateBasketItemQuantityRequest(
    Guid Id,
    int Quantity
) : IRequest<Result<UpdateBasketItemQuantityResponse>>;