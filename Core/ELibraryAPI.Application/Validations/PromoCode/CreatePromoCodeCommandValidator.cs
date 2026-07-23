using ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;
using FluentValidation;

public sealed class CreatePromoCodeCommandValidator : AbstractValidator<CreatePromoCodeCommandRequest>
{
    public CreatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("kodu boş ola bilməz.").MaximumLength(20).WithMessage("kodu maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.DiscountPercent).InclusiveBetween(1, 100);
        RuleFor(x => x.UsageLimit).GreaterThan(0).WithMessage("Usage Limit {ComparisonValue}-dan böyük olmalıdır.");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start tarixi boş ola bilməz.");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End tarixi boş ola bilməz.").GreaterThan(x => x.StartDate).WithMessage("End tarixi {ComparisonValue}-dan böyük olmalıdır.");
    }
}