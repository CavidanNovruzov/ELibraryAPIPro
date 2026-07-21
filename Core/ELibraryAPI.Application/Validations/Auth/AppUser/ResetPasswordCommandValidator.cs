using ELibraryAPI.Application.Features.Commands.Auth.ResetPassword;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommandRequest>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId boş ola bilməz.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token boş ola bilməz.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword boş ola bilməz.")
            .MinimumLength(8)
            .WithMessage("NewPassword minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(128)
            .WithMessage("NewPassword maksimum {MaxLength} simvol ola bilər.");
    }
}