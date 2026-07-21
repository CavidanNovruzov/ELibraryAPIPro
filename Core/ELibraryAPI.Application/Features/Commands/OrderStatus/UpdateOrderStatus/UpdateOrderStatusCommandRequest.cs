using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdateOrderStatusCommandResponse>>;
