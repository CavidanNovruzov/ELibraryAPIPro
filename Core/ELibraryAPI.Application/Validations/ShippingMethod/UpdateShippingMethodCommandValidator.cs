using ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.ShippingMethod;

public sealed class UpdateShippingMethodCommandValidator : AbstractValidator<UpdateShippingMethodCommandRequest>
{
    public UpdateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}
