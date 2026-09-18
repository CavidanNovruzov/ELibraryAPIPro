using ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class GetAuthorsByAlphabetQueryValidator : AbstractValidator<GetAuthorsByAlphabetQueryRequest>
{
    public GetAuthorsByAlphabetQueryValidator()
    {
        RuleFor(x => x.Letter)
            .NotEmpty()
            .WithMessage("Hərf boş ola bilməz.")
            .MaximumLength(1)
            .WithMessage("Hərf yalnız bir simvol olmalıdır.");
    }
}