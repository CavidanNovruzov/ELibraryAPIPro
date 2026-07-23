using ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PaymentMethod;

public sealed class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommandRequest>
{
    public UpdatePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(100).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
    }
}
