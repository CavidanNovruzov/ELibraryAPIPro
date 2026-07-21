
namespace ELibraryAPI.Application.Dtos.Payment;

public record PaymentResultDto(
 bool IsSuccess,
 string? TransactionId, 
 string? PaymentUrl,    
 string? ErrorMessage
);
