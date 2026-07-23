using ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order;

public sealed class ChangeOrderStatusCommandValidator : AbstractValidator<ChangeOrderStatusCommandRequest>
{
    public ChangeOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Sifariş ID-si mütləqdir.");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("Yeni status ID-si mütləqdir.");
    }
}