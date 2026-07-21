using ELibraryAPI.Application.Features.Commands.Product.SetProductActive;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class SetProductActiveCommandValidator : AbstractValidator<SetProductActiveCommandRequest>
{
    public SetProductActiveCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

