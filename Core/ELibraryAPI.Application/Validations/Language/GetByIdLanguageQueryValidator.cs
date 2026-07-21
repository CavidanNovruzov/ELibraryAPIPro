using ELibraryAPI.Application.Features.Queries.Language.GetByIdLanguage;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Language;

public sealed class GetByIdLanguageQueryValidator : AbstractValidator<GetByIdLanguageQueryRequest>
{
    public GetByIdLanguageQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
