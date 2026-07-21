using ELibraryAPI.Application.Dtos.Payment;

namespace ELibraryAPI.Application.Abstractions.Services;

public interface IPaymentService
{
    Task<PaymentResultDto> InitializePaymentAsync(PaymentRequestDto requestDto, CancellationToken ct = default);
    Task<PaymentStatusResultDto> CheckTransactionStatusAsync(string transactionId, CancellationToken ct = default);
}
