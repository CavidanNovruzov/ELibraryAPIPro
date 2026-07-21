using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.DeleteOrderStatus;

public sealed record DeleteOrderStatusCommandRequest(Guid Id) : IRequest<Result>;
