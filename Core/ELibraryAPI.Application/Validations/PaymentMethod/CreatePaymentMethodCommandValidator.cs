using ELibraryAPI.Application.Features.Commands.PaymentMethod.CreatePaymentMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PaymentMethod;

public sealed class CreatePaymentMethodCommandValidator : AbstractValidator<CreatePaymentMethodCommandRequest>
{
    public CreatePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
