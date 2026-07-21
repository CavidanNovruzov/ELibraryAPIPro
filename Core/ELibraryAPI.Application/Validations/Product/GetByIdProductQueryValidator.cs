using ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Product;

public sealed class GetByIdProductQueryValidator : AbstractValidator<GetByIdProductQueryRequest>
{
    public GetByIdProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Valid Product ID is required.");
    }
}
