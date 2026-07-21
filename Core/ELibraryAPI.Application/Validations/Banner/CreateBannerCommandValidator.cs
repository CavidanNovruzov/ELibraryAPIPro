using ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Banner;

public sealed class CreateBannerCommandValidator : AbstractValidator<CreateBannerCommandRequest>
{
    public CreateBannerCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("Title maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.RedirectUrl)
            .MaximumLength(500)
            .WithMessage("RedirectUrl maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Order 0-dan böyük və ya bərabər olmalıdır.");
    }
}