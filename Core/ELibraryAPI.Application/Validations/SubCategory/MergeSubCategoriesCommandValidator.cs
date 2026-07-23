using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;

public sealed class MergeSubCategoriesCommandValidator : AbstractValidator<MergeSubCategoriesCommandRequest>
{
    public MergeSubCategoriesCommandValidator()
    {
        RuleFor(x => x.SourceSubCategoryId)
            .NotEmpty().WithMessage("Mənbə alt-kateqoriya ID-si mütləqdir.");

        RuleFor(x => x.TargetSubCategoryId)
            .NotEmpty().WithMessage("Hədəf alt-kateqoriya ID-si mütləqdir.")
            .NotEqual(x => x.SourceSubCategoryId).WithMessage("Mənbə və hədəf alt-kateqoriyaları eyni ola bilməz.");
    }
}