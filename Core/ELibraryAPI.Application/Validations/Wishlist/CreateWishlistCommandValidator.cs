using ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Wishlist;

public sealed class CreateWishlistCommandValidator : AbstractValidator<CreateWishlistCommandRequest>
{
    public CreateWishlistCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
