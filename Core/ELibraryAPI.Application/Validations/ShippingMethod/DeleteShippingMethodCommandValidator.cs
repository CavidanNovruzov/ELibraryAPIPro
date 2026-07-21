using ELibraryAPI.Application.Features.Commands.ShippingMethod.DeleteShippingMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.ShippingMethod;

public sealed class DeleteShippingMethodCommandValidator : AbstractValidator<DeleteShippingMethodCommandRequest>
{
    public DeleteShippingMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
