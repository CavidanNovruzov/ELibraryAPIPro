using ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Banner;

public sealed class CreateBannerCommandValidator : AbstractValidator<CreateBannerCommandRequest>
{
    public CreateBannerCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Başlıq boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("Başlıq maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.RedirectUrl)
            .MaximumLength(500)
            .WithMessage("Yönləndirmə keçidi (RedirectUrl) maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sıralama 0-dan böyük və ya bərabər olmalıdır.");
    }
}