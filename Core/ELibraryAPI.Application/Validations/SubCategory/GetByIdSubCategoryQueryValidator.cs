using ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class GetByIdSubCategoryQueryValidator : AbstractValidator<GetByIdSubCategoryQueryRequest>
{
    public GetByIdSubCategoryQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sub-category ID is required.")
            .NotEqual(Guid.Empty).WithMessage("A valid Sub-category ID must be provided.");
    }
}
