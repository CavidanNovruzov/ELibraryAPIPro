using ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommandRequest>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
