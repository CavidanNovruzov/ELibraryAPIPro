using ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Product;

public sealed class GetByIdProductQueryValidator : AbstractValidator<GetByIdProductQueryRequest>
{
    public GetByIdProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Məhsul ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Düzgün Məhsul ID-si mütləqdir.");
    }
}
