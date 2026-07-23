using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Dtos.Payment;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Transaction.InitializeTransaction;

public sealed class InitializeTransactionCommandHandler : IRequestHandler<InitializeTransactionCommandRequest, Result<InitializeTransactionCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public InitializeTransactionCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Result<InitializeTransactionCommandResponse>> Handle(InitializeTransactionCommandRequest request, CancellationToken ct)
    {
        var orderReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>();
        var transactionWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Transaction, Guid>();

        var order = await orderReadRepository.GetByIdAsync(
            request.OrderId,
            tracking: false,
            ct: ct);

        if (order == null)
        {
            return Result<InitializeTransactionCommandResponse>.NotFound("Sifariş tapılmadı..");
        }

        var paymentRequest = new PaymentRequestDto(order.Id, order.TotalAmount);
        var paymentResult = await _paymentService.InitializePaymentAsync(paymentRequest, ct);

        if (!paymentResult.IsSuccess)
        {
            return Result<InitializeTransactionCommandResponse>.Failure(
                $"Payment provider error: {paymentResult.ErrorMessage}",
                ErrorType.BadRequest);
        }

        var transaction = new Domain.Entities.Concrete.Transaction
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Currency = "AZN",
            PaymentProvider = request.PaymentProvider,
            TransactionId = paymentResult.TransactionId,
            Status = TransactionStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        await transactionWriteRepository.AddAsync(transaction, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<InitializeTransactionCommandResponse>.Success(
            new InitializeTransactionCommandResponse(paymentResult.PaymentUrl),
            "Transaction initialized uğurla tamamlandı.");
    }
}