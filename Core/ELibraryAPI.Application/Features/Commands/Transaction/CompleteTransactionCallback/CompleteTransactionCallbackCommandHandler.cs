using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Transaction.CompleteTransactionCallback;

public sealed class CompleteTransactionCallbackCommandHandler : IRequestHandler<CompleteTransactionCallbackCommandRequest, Result<CompleteTransactionCallbackCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTransactionCallbackCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompleteTransactionCallbackCommandResponse>> Handle(CompleteTransactionCallbackCommandRequest request, CancellationToken ct)
    {
        var transactionReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Transaction, Guid>();
        var transactionWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Transaction, Guid>();
        var orderWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>();

        var transaction = await transactionReadRepository.GetSingleAsync(
            t => t.TransactionId == request.TransactionId,
            tracking: true,
            ct: ct,
            includes: t => t.Order);

        if (transaction == null)
        {
            return Result<CompleteTransactionCallbackCommandResponse>.NotFound("Transaction not found.");
        }

        if (transaction.Status != TransactionStatus.Pending)
        {
            return Result<CompleteTransactionCallbackCommandResponse>.Failure(
                "This transaction has already been processed.",
                ErrorType.BadRequest);
        }

        transaction.ProviderResponse = request.ProviderResponse;

        if (request.IsSuccess)
        {
            transaction.Status = TransactionStatus.Success;
            if (transaction.Order != null)
            {
                orderWriteRepository.Update(transaction.Order);
            }
        }
        else
        {
            transaction.Status = TransactionStatus.Failed;
            if (transaction.Order != null)
            {
                orderWriteRepository.Update(transaction.Order);
            }
        }

        transactionWriteRepository.Update(transaction);
        await _unitOfWork.SaveAsync(ct);

        string responseMessage = request.IsSuccess
            ? "Transaction completed successfully."
            : "Transaction failed or was rejected by the provider.";

        return Result<CompleteTransactionCallbackCommandResponse>.Success(
            new CompleteTransactionCallbackCommandResponse(transaction.OrderId, true),
            responseMessage);
    }
}
