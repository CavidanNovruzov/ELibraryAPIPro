using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Stock.DeleteStock;

public sealed record DeleteStockCommandRequest(Guid Id) : IRequest<Result>;
