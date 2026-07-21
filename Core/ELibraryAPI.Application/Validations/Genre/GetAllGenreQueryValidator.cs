using ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Genre;

public sealed class GetAllGenreQueryValidator : AbstractValidator<GetAllGenreQueryRequest>
{
    public GetAllGenreQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə nömrəsi ən azı {ComparisonValue} olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}