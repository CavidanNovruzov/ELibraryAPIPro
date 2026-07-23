using ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class GetAllAuthorQueryValidator : AbstractValidator<GetAllAuthorQueryRequest>
{
    public GetAllAuthorQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə 1-dən böyük və ya bərabər olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Ölçü {From} və {To} arasında olmalıdır.");
    }
}