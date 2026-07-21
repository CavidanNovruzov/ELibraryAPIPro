using ELibraryAPI.Application.Features.Commands.Banner.DeleteBanner;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Banner;

public sealed class DeleteBannerCommandValidator : AbstractValidator<DeleteBannerCommandRequest>
{
    public DeleteBannerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Banner ID silinmə üçün tələb olunur.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Banner ID.");
    }
}