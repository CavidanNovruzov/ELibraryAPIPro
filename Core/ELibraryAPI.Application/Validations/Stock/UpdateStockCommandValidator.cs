using ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Stock;

public sealed class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommandRequest>
{
    public UpdateStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.BranchId).NotEmpty().WithMessage("Filial ID-si boş ola bilməz.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("miqdarı {ComparisonValue}-dan böyük olmalıdır.");
    }
}
