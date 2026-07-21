using ELibraryAPI.Application.Features.Commands.OrderStatus.DeleteOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.OrderStatus;

public sealed class DeleteOrderStatusCommandValidator : AbstractValidator<DeleteOrderStatusCommandRequest>
{
    public DeleteOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
