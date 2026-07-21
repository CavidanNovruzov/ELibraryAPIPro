using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;

public sealed class GetStockByProductIdQueryHandler : IRequestHandler<GetStockByProductIdQueryRequest, Result<GetStockByProductIdQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStockByProductIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetStockByProductIdQueryResponse>> Handle(GetStockByProductIdQueryRequest request, CancellationToken cancellationToken)
    {
        var stocks = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Stock, Guid>()
            .GetAll(tracking: false)
            .Where(s => s.ProductId == request.ProductId)
            .Select(s => new StockByProductDto(
                s.BranchId,
                s.Branch.Name,
                s.Branch.Location ?? string.Empty,
                s.Quantity,
                s.Quantity > 0
            ))
            .ToListAsync(cancellationToken);

        if (stocks == null || !stocks.Any())
            return Result<GetStockByProductIdQueryResponse>.Failure("No stock information found for this product.");

        return Result<GetStockByProductIdQueryResponse>.Success(new GetStockByProductIdQueryResponse(stocks));
    }
}
