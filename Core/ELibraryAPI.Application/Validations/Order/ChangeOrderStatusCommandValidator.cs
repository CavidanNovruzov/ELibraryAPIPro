using ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class ChangeOrderStatusCommandValidator : AbstractValidator<ChangeOrderStatusCommandRequest>
{
    public ChangeOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required.");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("New status Id is required.");
    }
}