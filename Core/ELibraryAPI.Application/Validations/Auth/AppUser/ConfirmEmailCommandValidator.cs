using ELibraryAPI.Application.Features.Commands.Auth.ConfirmEmail;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommandRequest>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId boş ola bilməz.")
            .MaximumLength(64)
            .WithMessage("UserId maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token boş ola bilməz.")
            .MaximumLength(2048)
            .WithMessage("Token maksimum {MaxLength} simvol ola bilər.");
    }
}