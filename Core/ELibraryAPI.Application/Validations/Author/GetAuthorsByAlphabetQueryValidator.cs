using ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class GetAuthorsByAlphabetQueryValidator : AbstractValidator<GetAuthorsByAlphabetQueryRequest>
{
    public GetAuthorsByAlphabetQueryValidator()
    {
        RuleFor(x => x.Letter)
            .NotEmpty()
            .WithMessage("Letter boş ola bilməz.")
            .Must(char.IsLetter)
            .WithMessage("Zəhmət olmasa, düzgün bir hərf daxil edin.");
    }
}