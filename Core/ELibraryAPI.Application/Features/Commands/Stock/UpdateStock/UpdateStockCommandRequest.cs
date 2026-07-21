using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;

public sealed record UpdateStockCommandRequest(
    Guid Id,
    Guid BranchId,
    Guid ProductId,
    int Quantity
) : IRequest<Result<UpdateStockCommandResponse>>;
