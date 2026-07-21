using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Transaction.CompleteTransactionCallback;

public sealed record CompleteTransactionCallbackCommandRequest(
    string TransactionId,
    bool IsSuccess,
    string? ProviderResponse
) : IRequest<Result<CompleteTransactionCallbackCommandResponse>>;
