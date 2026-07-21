using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Transaction.SyncTransactionStatus;

public sealed record SyncTransactionStatusCommandRequest(
    Guid TransactionId
) : IRequest<Result<SyncTransactionStatusResponse>>;
