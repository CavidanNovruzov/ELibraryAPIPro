using ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.OrderStatus;

public sealed class CreateOrderStatusCommandValidator : AbstractValidator<CreateOrderStatusCommandRequest>
{
    public CreateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
