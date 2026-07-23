using ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.SubCategory;

public sealed class GetByIdSubCategoryQueryValidator : AbstractValidator<GetByIdSubCategoryQueryRequest>
{
    public GetByIdSubCategoryQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Alt-kateqoriya ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Düzgün Alt-kateqoriya ID-si təmin edilməlidir.");
    }
}
