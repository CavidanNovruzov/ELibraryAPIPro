using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.DeleteBasketItem;

public sealed record DeleteBasketItemCommandRequest(Guid Id) : IRequest<Result>;
