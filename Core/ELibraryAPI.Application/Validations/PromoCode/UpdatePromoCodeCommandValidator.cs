using ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PromoCode;

public sealed class UpdatePromoCodeCommandValidator : AbstractValidator<UpdatePromoCodeCommandRequest>
{
    public UpdatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0,100);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.UsageLimit).GreaterThanOrEqualTo(0);
    }
}
