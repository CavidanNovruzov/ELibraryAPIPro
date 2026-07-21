using ELibraryAPI.Domain.Enums;

namespace ELibraryAPI.Application.Features.Queries.Transaction.GetAllTransaction;

public sealed record GetAllTransactionQueryResponse(
    List<TransactionListDto> Transactions,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record TransactionListDto(
    Guid              Id,
    Guid              OrderId,
    decimal           Amount,
    TransactionStatus Status,
    bool              IsSuccess,
    DateTime          CreatedDate
);
