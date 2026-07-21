using ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PaymentMethod;

public sealed class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommandRequest>
{
    public UpdatePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
