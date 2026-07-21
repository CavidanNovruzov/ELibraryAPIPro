using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Dtos.Payment;

    public record PaymentRequestDto(
    Guid OrderId,
    decimal Amount,
    string Currency = "AZN"
    );

