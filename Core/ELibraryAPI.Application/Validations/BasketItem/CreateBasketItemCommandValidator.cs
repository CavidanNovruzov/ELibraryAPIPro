using ELibraryAPI.Application.Features.Commands.BasketItem.CreateBasketItem;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BasketItem;

public sealed class CreateBasketItemCommandValidator : AbstractValidator<CreateBasketItemCommandRequest>
{
    public CreateBasketItemCommandValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty().WithMessage("Səbət ID-si boş ola bilməz.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Miqdar {ComparisonValue}-dan böyük olmalıdır.")
            .LessThanOrEqualTo(100).WithMessage("Bir əməliyyatda maksimum 100 ədəd əlavə edilə bilər.");
    }
}
