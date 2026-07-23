using ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.ShippingMethod;

public sealed class CreateShippingMethodCommandValidator : AbstractValidator<CreateShippingMethodCommandRequest>
{
    public CreateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(100).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("qiyməti {ComparisonValue}-dan böyük olmalıdır.");
    }
}
