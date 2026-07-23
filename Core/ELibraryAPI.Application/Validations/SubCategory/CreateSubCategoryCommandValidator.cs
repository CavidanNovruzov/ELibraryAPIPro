using ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommandRequest>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kateqoriya ID-si boş ola bilməz.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(200).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
    }
}
