using ELibraryAPI.Application.Features.Commands.Order.DeleteOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommandRequest>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Order Id is required.");
    }
}