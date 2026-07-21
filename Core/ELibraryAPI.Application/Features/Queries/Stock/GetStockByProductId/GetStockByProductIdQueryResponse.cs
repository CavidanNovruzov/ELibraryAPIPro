
namespace ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;

public sealed record GetStockByProductIdQueryResponse(List<StockByProductDto> BranchStocks);

public sealed record StockByProductDto(
    Guid BranchId,
    string BranchName,
    string BranchLocation,
    int Quantity,
    bool IsAvailable
);
