using ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PromoCode;

public sealed class UpdatePromoCodeCommandValidator : AbstractValidator<UpdatePromoCodeCommandRequest>
{
    public UpdatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Code).NotEmpty().WithMessage("kodu boş ola bilməz.").MaximumLength(50).WithMessage("kodu maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0,100);
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End tarixi boş ola bilməz.").GreaterThan(x => x.StartDate).WithMessage("End tarixi {ComparisonValue}-dan böyük olmalıdır.");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start tarixi boş ola bilməz.");
        RuleFor(x => x.UsageLimit).GreaterThanOrEqualTo(0).WithMessage("Usage Limit {ComparisonValue}-dan böyük olmalıdır.");
    }
}
