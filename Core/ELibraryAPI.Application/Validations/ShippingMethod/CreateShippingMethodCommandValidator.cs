using ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.ShippingMethod;

public sealed class CreateShippingMethodCommandValidator : AbstractValidator<CreateShippingMethodCommandRequest>
{
    public CreateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}
