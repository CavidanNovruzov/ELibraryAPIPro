using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Basket;

// KÖÇÜRÜLDÜ: Validations/WishlistItem/MoveToBasketCommandValidator.cs faylından bura.
// Səbəb: Bu validator ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket
// namespace-indəki MoveToBasketCommandRequest-i doğrulayır (WishlistItem-in öz
// MoveToBasket command-i yoxdur, WishlistItemsController birbaşa Basket modulunun
// command-ini çağırır). Validator-ın əvvəlki yeri (Validations/WishlistItem/,
// namespace Features.Commands.WishlistItem.MoveToBasket) ilə əsl tipin yeri
// arasındakı uyğunsuzluq gələcəkdə "ambiguous reference" riski yaradırdı.
//
// SİLİNMƏLİ KÖHNƏ FAYL: Validations/WishlistItem/MoveToBasketCommandValidator.cs
public sealed class MoveToBasketCommandValidator : AbstractValidator<MoveToBasketCommandRequest>
{
    public MoveToBasketCommandValidator()
    {
        RuleFor(x => x.WishlistItemId).NotEmpty().WithMessage("Arzu siyahısı elementi ID-si mütləqdir.");
    }
}
