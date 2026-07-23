using ELibraryAPI.Application.Features.Commands.Banner.UpdateBanner;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Banner;

public sealed class UpdateBannerCommandValidator : AbstractValidator<UpdateBannerCommandRequest>
{
    public UpdateBannerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id boş ola bilməz.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sıralama {ComparisonValue}-dan kiçik ola bilməz.");
    }
}