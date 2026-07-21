using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Transaction.InitializeTransaction;

public sealed record InitializeTransactionCommandRequest(
    Guid OrderId,
    string PaymentProvider = "KapitalBank"
) : IRequest<Result<InitializeTransactionCommandResponse>>;
