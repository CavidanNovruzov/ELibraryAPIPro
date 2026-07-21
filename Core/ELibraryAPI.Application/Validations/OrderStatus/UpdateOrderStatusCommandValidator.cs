using ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.OrderStatus;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommandRequest>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
