using ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Genre;

public sealed class GetByIdGenreQueryValidator : AbstractValidator<GetByIdGenreQueryRequest>
{
    public GetByIdGenreQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Janr ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Janr ID-si.");
    }
}