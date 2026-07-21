using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Basket.CreateBasket;

public sealed record CreateBasketCommandRequest : IRequest<Result<CreateBasketCommandResponse>>;
