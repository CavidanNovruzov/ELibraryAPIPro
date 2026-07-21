using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.CreateBasketItem;

public sealed record CreateBasketItemCommandRequest(
    Guid BasketId,
    Guid ProductId,
    int Quantity
) : IRequest<Result<CreateBasketItemCommandResponse>>;
