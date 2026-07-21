using ELibraryAPI.Application.Features.Commands.Transaction.InitializeTransaction;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Transaction
{
    public sealed class InitializeTransactionCommandValidator : AbstractValidator<InitializeTransactionCommandRequest>
    {
        public InitializeTransactionCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Sifariş ID mütləq qeyd olunmalıdır.")
                .NotEqual(Guid.Empty).WithMessage("Yanlış Sifariş ID formatı.");

            RuleFor(x => x.PaymentProvider)
                .NotEmpty().WithMessage("Ödəniş provayderi mütləq qeyd olunmalıdır.")
                .MaximumLength(50).WithMessage("Ödəniş provayderinin adı 50 simvoldan çox ola bilməz.");
        }
    }
}
