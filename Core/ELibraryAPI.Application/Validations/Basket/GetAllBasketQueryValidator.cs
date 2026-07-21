using ELibraryAPI.Application.Features.Queries.Basket.GetAllBasket;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Basket;

public sealed class GetAllBasketQueryValidator : AbstractValidator<GetAllBasketQueryRequest>
{
    public GetAllBasketQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə nömrəsi ən azı {ComparisonValue} olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}