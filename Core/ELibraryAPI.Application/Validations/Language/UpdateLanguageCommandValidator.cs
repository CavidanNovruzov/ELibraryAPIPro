using ELibraryAPI.Application.Features.Commands.Language.UpdateLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language;

public sealed class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommandRequest>
{
    public UpdateLanguageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Language Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Language name is required.")
            .MaximumLength(50).WithMessage("Language name cannot exceed 50 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Language code is required.")
            .Length(2).WithMessage("Language code must be exactly 2 characters (e.g., az, en, ru).");
    }
}