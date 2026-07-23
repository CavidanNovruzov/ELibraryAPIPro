using ELibraryAPI.Application.Features.Queries.Language.GetAllLanguage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Language
{
    public sealed class GetAllLanguageQueryValidator : AbstractValidator<GetAllLanguageQueryRequest>
    {
        public GetAllLanguageQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page {ComparisonValue}-dan böyük olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
