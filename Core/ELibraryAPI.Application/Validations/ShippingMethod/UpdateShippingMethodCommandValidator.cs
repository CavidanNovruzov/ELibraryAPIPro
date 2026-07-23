using ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.ShippingMethod;

public sealed class UpdateShippingMethodCommandValidator : AbstractValidator<UpdateShippingMethodCommandRequest>
{
    public UpdateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(100).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("qiyməti {ComparisonValue}-dan böyük olmalıdır.");
    }
}
