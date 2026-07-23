using ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommandRequest>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");
        RuleFor(x => x.OrderStatusId).NotEmpty().WithMessage("Sifariş statusu ID-si boş ola bilməz.");
        RuleFor(x => x.ShippingMethodId).NotEmpty().WithMessage("Shipping Method ID-si boş ola bilməz.");
        RuleFor(x => x.OrderNote).MaximumLength(500).WithMessage("Sifariş Note maksimum {MaxLength} simvol ola bilər.");
    }
}