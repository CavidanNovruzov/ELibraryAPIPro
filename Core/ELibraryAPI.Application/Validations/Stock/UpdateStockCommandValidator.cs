using ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Stock;

public sealed class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommandRequest>
{
    public UpdateStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
