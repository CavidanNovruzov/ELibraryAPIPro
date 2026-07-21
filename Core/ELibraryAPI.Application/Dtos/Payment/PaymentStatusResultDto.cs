
namespace ELibraryAPI.Application.Dtos.Payment;

public record PaymentStatusResultDto(
    bool IsSuccess,
    string Status,
    string? ErrorMessage
);
