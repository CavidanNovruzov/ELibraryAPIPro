using ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.OrderStatus;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommandRequest>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(100).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
    }
}
