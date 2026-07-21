using ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.WishlistItem;

public sealed class UpdateWishlistItemCommandValidator : AbstractValidator<UpdateWishlistItemCommandRequest>
{
    public UpdateWishlistItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WishlistId).NotEmpty();
    }
}
