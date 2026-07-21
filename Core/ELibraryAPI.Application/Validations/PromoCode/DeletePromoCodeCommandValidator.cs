using ELibraryAPI.Application.Features.Commands.PromoCode.DeletePromoCode;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PromoCode;

public sealed class DeletePromoCodeCommandValidator : AbstractValidator<DeletePromoCodeCommandRequest>
{
    public DeletePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
