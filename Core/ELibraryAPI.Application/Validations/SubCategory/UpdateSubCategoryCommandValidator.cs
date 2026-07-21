using ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommandRequest>
{
    public UpdateSubCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
