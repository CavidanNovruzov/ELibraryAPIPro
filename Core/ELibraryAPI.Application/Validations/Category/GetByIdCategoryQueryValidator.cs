using ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Category;

public sealed class GetByIdCategoryQueryValidator : AbstractValidator<GetByIdCategoryQueryRequest>
{
    public GetByIdCategoryQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kateqoriya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Kateqoriya ID-si.");
    }
}