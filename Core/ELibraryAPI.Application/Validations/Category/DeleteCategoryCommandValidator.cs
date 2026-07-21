using ELibraryAPI.Application.Features.Commands.Category.DeleteCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommandRequest>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kateqoriya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Kateqoriya ID-si.");
    }
}