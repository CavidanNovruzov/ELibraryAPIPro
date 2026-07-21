using ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.WishlistItem;

public sealed class DeleteWishlistItemCommandValidator : AbstractValidator<DeleteWishlistItemCommandRequest>
{
    public DeleteWishlistItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
