using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.DeleteOrder;

public sealed record DeleteOrderCommandRequest(Guid Id) : IRequest<Result>;
