using ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.WishlistItem;

public sealed class CreateWishlistItemCommandValidator : AbstractValidator<CreateWishlistItemCommandRequest>
{
    public CreateWishlistItemCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WishlistId).NotEmpty();
    }
}
