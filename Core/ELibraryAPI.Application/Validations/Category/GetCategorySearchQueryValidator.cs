using ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class GetCategorySearchQueryValidator : AbstractValidator<GetCategorySearchQueryRequest>
{
    public GetCategorySearchQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .WithMessage("Axtarış mətni boş ola bilməz.")
            .MinimumLength(2)
            .WithMessage("Axtarış mətni ən azı {MinLength} simvol olmalıdır.");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Səhifə nömrəsi 0-dan böyük olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 50)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}