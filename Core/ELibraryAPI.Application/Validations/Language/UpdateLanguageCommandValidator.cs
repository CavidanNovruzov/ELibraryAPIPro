using ELibraryAPI.Application.Features.Commands.Language.UpdateLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language;

public sealed class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommandRequest>
{
    public UpdateLanguageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Dil ID-si mütləqdir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Dil adı mütləqdir.")
            .MaximumLength(50).WithMessage("Dil adı 50 simvoldan çox ola bilməz.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Dil kodu mütləqdir.")
            .Length(2).WithMessage("Dil kodu dəqiq 2 simvol olmalıdır (məs: az, en, ru).");
    }
}