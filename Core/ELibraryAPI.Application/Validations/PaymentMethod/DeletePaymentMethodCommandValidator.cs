using ELibraryAPI.Application.Features.Commands.PaymentMethod.DeletePaymentMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PaymentMethod;

public sealed class DeletePaymentMethodCommandValidator : AbstractValidator<DeletePaymentMethodCommandRequest>
{
    public DeletePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
