

using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;

public sealed record MoveToBasketCommandRequest(Guid WishlistItemId) : IRequest<Result>;

