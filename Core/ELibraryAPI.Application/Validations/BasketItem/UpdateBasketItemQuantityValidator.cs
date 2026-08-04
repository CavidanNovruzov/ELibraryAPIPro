using ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BasketItem;

public sealed class UpdateBasketItemQuantityValidator : AbstractValidator<UpdateBasketItemQuantityRequest>
{
    public UpdateBasketItemQuantityValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Səbət elementi ID-si boş ola bilməz.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Miqdar {ComparisonValue}-dan böyük olmalıdır.")
            .LessThanOrEqualTo(100).WithMessage("Bir məhsuldan maksimum 100 ədəd ola bilər.");
    }
}
