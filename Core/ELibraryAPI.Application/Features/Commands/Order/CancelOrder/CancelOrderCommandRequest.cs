
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.CancelOrder;

public sealed record CancelOrderCommandRequest(Guid Id) : IRequest<Result>;
