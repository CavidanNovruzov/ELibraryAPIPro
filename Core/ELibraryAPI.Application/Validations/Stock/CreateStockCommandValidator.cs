using ELibraryAPI.Application.Features.Commands.Stock.CreateStock;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Stock;

public sealed class CreateStockCommandValidator : AbstractValidator<CreateStockCommandRequest>
{
    public CreateStockCommandValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
