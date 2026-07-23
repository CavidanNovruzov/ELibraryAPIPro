using ELibraryAPI.Application.Features.Commands.Language.CreateLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language;

public sealed class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommandRequest>
{
    public CreateLanguageCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Dil adı mütləqdir.")
            .MaximumLength(50).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Dil kodu mütləqdir.")
            .Length(2).WithMessage("Dil kodu dəqiq 2 simvol olmalıdır (məs: az, en).");
    }
}