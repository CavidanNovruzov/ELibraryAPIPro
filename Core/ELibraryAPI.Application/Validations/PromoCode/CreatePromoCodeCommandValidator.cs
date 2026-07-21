using ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;
using FluentValidation;

public sealed class CreatePromoCodeCommandValidator : AbstractValidator<CreatePromoCodeCommandRequest>
{
    public CreatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(1, 100);
        RuleFor(x => x.UsageLimit).GreaterThan(0);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}