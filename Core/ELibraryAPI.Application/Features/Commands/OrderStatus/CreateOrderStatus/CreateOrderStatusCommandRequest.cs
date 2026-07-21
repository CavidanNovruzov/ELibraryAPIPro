using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;

public sealed record CreateOrderStatusCommandRequest(
    string Name
) : IRequest<Result<CreateOrderStatusCommandResponse>>;
