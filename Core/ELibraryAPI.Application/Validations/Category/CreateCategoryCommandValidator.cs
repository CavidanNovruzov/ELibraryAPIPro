using ELibraryAPI.Application.Features.Commands.Category.CreateCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommandRequest>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Kateqoriya adı boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("Kateqoriya adı maksimum {MaxLength} simvol ola bilər.");
    }
}