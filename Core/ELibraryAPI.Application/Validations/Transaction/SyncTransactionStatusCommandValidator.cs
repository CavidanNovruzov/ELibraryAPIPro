using ELibraryAPI.Application.Features.Commands.Transaction.SyncTransactionStatus;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Transaction;

public sealed class SyncTransactionStatusCommandValidator : AbstractValidator<SyncTransactionStatusCommandRequest>
{
    public SyncTransactionStatusCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Tranzaksiya ID mütləq qeyd olunmalıdır.")
            .NotEqual(Guid.Empty).WithMessage("Yanlış tranzaksiya ID formatı.");
    }
}
