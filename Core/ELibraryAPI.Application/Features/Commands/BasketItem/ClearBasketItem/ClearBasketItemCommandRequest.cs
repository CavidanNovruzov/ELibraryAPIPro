using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.ClearBasketItem;

public sealed record ClearBasketItemCommandRequest(
    Guid BasketId
) : IRequest<Result<ClearBasketItemCommandResponse>>;