using ELibraryAPI.Application.Features.Commands.Stock.DeleteStock;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Stock;

public sealed class DeleteStockCommandValidator : AbstractValidator<DeleteStockCommandRequest>
{
    public DeleteStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
