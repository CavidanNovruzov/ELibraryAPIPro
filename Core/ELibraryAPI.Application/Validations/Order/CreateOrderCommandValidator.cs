using ELibraryAPI.Application.Features.Commands.Order.CreateOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId).NotEmpty();
        RuleFor(x => x.ShippingMethodId).NotEmpty();
        RuleFor(x => x.UserAddressId).NotEmpty();
        RuleFor(x => x.OrderNote).MaximumLength(500);
        RuleFor(x => x.PromoCode).MaximumLength(50);
    }
}
