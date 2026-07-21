using ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Stock;

public sealed class GetStockByProductIdQueryValidator : AbstractValidator<GetStockByProductIdQueryRequest>
{
    public GetStockByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.")
            .NotEqual(Guid.Empty).WithMessage("A valid Product ID must be provided.");
    }
}
