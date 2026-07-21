using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;

public sealed class GetAllStockQueryHandler : IRequestHandler<GetAllStockQueryRequest, Result<GetAllStockQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllStockQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllStockQueryResponse>> Handle(GetAllStockQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Stock, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var stocks = await query
            .OrderBy(s => s.Product.Title)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(s => new StockListDto(
                s.Id,
                s.ProductId,
                s.Product.Title,
                s.BranchId,
                s.Branch.Name,
                s.Quantity,
                s.Quantity > 0
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllStockQueryResponse>.Success(
            new GetAllStockQueryResponse(stocks, totalCount, request.Page, request.Size, totalPages));
    }
}