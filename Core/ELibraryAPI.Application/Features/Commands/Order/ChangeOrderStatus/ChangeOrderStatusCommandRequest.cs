using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;

public sealed record ChangeOrderStatusCommandRequest(
    Guid OrderId,
    Guid StatusId
) : IRequest<Result>;