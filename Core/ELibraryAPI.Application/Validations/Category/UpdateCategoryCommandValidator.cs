using ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommandRequest>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Kateqoriya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz Kateqoriya ID-si.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kateqoriya adı boş ola bilməz.")
            .MaximumLength(200).WithMessage("Kateqoriya adı maksimum {MaxLength} simvol ola bilər.");
    }
}