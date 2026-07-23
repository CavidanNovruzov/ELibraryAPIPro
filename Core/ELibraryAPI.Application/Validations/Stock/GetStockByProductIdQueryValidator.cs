using ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Stock;

public sealed class GetStockByProductIdQueryValidator : AbstractValidator<GetStockByProductIdQueryRequest>
{
    public GetStockByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Məhsul ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Düzgün Məhsul ID-si təmin edilməlidir.");
    }
}
