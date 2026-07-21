using ELibraryAPI.Application.Features.Commands.SubCategory.DeleteSubCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class DeleteSubCategoryCommandValidator : AbstractValidator<DeleteSubCategoryCommandRequest>
{
    public DeleteSubCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
