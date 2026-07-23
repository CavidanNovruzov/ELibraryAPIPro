using ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.WishlistItem;

public sealed class UpdateWishlistItemCommandValidator : AbstractValidator<UpdateWishlistItemCommandRequest>
{
    public UpdateWishlistItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");
        RuleFor(x => x.WishlistId).NotEmpty().WithMessage("Arzu siyahısı ID-si boş ola bilməz.");
    }
}
