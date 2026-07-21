using ELibraryAPI.Application.Features.Commands.Transaction.CompleteTransactionCallback;
using FluentValidation;



namespace ELibraryAPI.Application.Validations.Transaction;

public sealed class CompleteTransactionCallbackCommandValidator : AbstractValidator<CompleteTransactionCallbackCommandRequest>
{
    public CompleteTransactionCallbackCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Provayderdən gələn tranzaksiya ID mütləq qeyd olunmalıdır.");
    }
}
