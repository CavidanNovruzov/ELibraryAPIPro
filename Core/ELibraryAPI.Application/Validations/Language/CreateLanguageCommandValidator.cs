using ELibraryAPI.Application.Features.Commands.Language.CreateLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language;

public sealed class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommandRequest>
{
    public CreateLanguageCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Language name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Language code is required.")
            .Length(2).WithMessage("Language code must be exactly 2 characters (e.g., az, en).");
    }
}