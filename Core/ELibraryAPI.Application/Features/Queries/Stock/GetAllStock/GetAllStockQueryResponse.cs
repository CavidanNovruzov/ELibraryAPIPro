namespace ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;

public sealed record GetAllStockQueryResponse(
    List<StockListDto> Stocks,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record StockListDto(
    Guid Id,
    Guid ProductId,
    string ProductTitle,
    Guid BranchId,
    string BranchName,
    int Quantity,
    bool IsAvailable
);
