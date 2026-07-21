using ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class GetAllCategoryQueryValidator : AbstractValidator<GetAllCategoryQueryRequest>
{
    public GetAllCategoryQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə nömrəsi ən azı {ComparisonValue} olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}