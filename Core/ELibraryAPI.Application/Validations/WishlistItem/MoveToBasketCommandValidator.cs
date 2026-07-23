using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;
using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.MoveToBasket;

public sealed class MoveToBasketCommandValidator : AbstractValidator<MoveToBasketCommandRequest>
{
    public MoveToBasketCommandValidator()
    {
        RuleFor(x => x.WishlistItemId).NotEmpty().WithMessage("Arzu siyahısı elementi ID-si mütləqdir.");
    }
}