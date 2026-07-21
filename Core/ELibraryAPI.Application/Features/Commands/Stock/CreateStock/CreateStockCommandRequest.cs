using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Stock.CreateStock;

public sealed record CreateStockCommandRequest(
    Guid BranchId,
    Guid ProductId,
    int Quantity
) : IRequest<Result<CreateStockCommandResponse>>;
