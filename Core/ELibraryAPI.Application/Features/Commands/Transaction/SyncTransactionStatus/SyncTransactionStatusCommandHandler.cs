
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Transaction.SyncTransactionStatus
{
    public sealed class SyncTransactionStatusCommandHandler : IRequestHandler<SyncTransactionStatusCommandRequest, Result<SyncTransactionStatusResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public SyncTransactionStatusCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Result<SyncTransactionStatusResponse>> Handle(SyncTransactionStatusCommandRequest request, CancellationToken ct)
        {
            var transactionReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Transaction, Guid>();
            var transactionWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Transaction, Guid>();
            var orderWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>();

            var transaction = await transactionReadRepository.GetByIdAsync(request.TransactionId, tracking: true, ct: ct, includes: t => t.Order);
            if (transaction == null) return Result<SyncTransactionStatusResponse>.NotFound("Transaction not found.");
            if (transaction.Status != TransactionStatus.Pending) return Result<SyncTransactionStatusResponse>.Success(new SyncTransactionStatusResponse(transaction.Status.ToString()), "Transaction already synchronized.");

            var bankResult = await _paymentService.CheckTransactionStatusAsync(transaction.TransactionId!, ct);
            if (!bankResult.IsSuccess) return Result<SyncTransactionStatusResponse>.Failure("Failed to fetch status from provider.", ErrorType.BadRequest);

            if (bankResult.Status == "APPROVED")
            {
                transaction.Status = TransactionStatus.Success;
                if (transaction.Order != null) orderWriteRepository.Update(transaction.Order);
            }
            else if (bankResult.Status == "DECLINED")
            {
                transaction.Status = TransactionStatus.Failed;
                if (transaction.Order != null) orderWriteRepository.Update(transaction.Order);
            }

            transactionWriteRepository.Update(transaction);
            await _unitOfWork.SaveAsync(ct);

            return Result<SyncTransactionStatusResponse>.Success(new SyncTransactionStatusResponse(transaction.Status.ToString()), "Status synchronized successfully.");
        }
    }
}
