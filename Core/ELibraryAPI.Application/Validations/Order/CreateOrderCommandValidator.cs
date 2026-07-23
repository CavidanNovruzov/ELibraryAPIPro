using ELibraryAPI.Application.Features.Commands.Order.CreateOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId).NotEmpty().WithMessage("Payment Method ID-si boş ola bilməz.");
        RuleFor(x => x.ShippingMethodId).NotEmpty().WithMessage("Shipping Method ID-si boş ola bilməz.");
        RuleFor(x => x.UserAddressId).NotEmpty().WithMessage("İstifadəçi Ünvan ID-si boş ola bilməz.");
        RuleFor(x => x.OrderNote).MaximumLength(500).WithMessage("Sifariş Note maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.PromoCode).MaximumLength(50).WithMessage("Promo kodu maksimum {MaxLength} simvol ola bilər.");
    }
}
