using ELibraryAPI.Application.Features.Commands.Wishlist.DeleteWishlist;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Wishlist;

public sealed class DeleteWishlistCommandValidator : AbstractValidator<DeleteWishlistCommandRequest>
{
    public DeleteWishlistCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
