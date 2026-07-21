using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;

public sealed class MergeSubCategoriesCommandValidator : AbstractValidator<MergeSubCategoriesCommandRequest>
{
    public MergeSubCategoriesCommandValidator()
    {
        RuleFor(x => x.SourceSubCategoryId)
            .NotEmpty().WithMessage("Source sub-category ID is required.");

        RuleFor(x => x.TargetSubCategoryId)
            .NotEmpty().WithMessage("Target sub-category ID is required.")
            .NotEqual(x => x.SourceSubCategoryId).WithMessage("Source and target sub-categories cannot be the same.");
    }
}