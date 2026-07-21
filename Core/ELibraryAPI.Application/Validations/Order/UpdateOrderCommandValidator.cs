using ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommandRequest>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrderStatusId).NotEmpty();
        RuleFor(x => x.ShippingMethodId).NotEmpty();
        RuleFor(x => x.OrderNote).MaximumLength(500);
    }
}