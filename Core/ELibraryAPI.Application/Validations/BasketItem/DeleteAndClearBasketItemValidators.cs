using ELibraryAPI.Application.Features.Commands.BasketItem.DeleteBasketItem;
using ELibraryAPI.Application.Features.Commands.BasketItem.ClearBasketItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BasketItem;

public sealed class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommandRequest>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Səbət elementi ID-si boş ola bilməz.");
    }
}

public sealed class ClearBasketItemCommandValidator : AbstractValidator<ClearBasketItemCommandRequest>
{
    public ClearBasketItemCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty().WithMessage("Səbət ID-si boş ola bilməz.");
    }
}
