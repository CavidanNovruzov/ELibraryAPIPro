using ELibraryAPI.Application.Abstractions.Services.Payment;
using ELibraryAPI.Application.Dtos.Payment;

namespace ELibraryAPI.Infrastructure.Services.Payment;

public class PaymentService : IPaymentService
{
    public async Task<PaymentResultDto> InitializePaymentAsync(PaymentRequestDto requestDto, CancellationToken ct = default)
    {
        var result = new PaymentResultDto(
            IsSuccess: true,
            TransactionId: Guid.NewGuid().ToString(),
            PaymentUrl: "https://checkout.example.com/pay/mock-transaction-id",
            ErrorMessage: null
        );

        return await Task.FromResult(result);
    }

    public async Task<PaymentStatusResultDto> CheckTransactionStatusAsync(string transactionId, CancellationToken ct = default)
    {
        var statusResult = new PaymentStatusResultDto(
            IsSuccess: true,
            Status: "SUCCESS",
            ErrorMessage: null
        );

        return await Task.FromResult(statusResult);
    }
}