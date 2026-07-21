using ELibraryAPI.Application.Features.Commands.Language.DeleteLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language;

public sealed class DeleteLanguageCommandValidator : AbstractValidator<DeleteLanguageCommandRequest>
{
    public DeleteLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
