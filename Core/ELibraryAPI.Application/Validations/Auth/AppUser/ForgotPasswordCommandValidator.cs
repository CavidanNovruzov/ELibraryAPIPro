using ELibraryAPI.Application.Features.Commands.Auth.ForgotPassword;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommandRequest>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email boş ola bilməz.")
            .EmailAddress()
            .WithMessage("Düzgün bir email ünvanı daxil edin.")
            .MaximumLength(256)
            .WithMessage("E-poçt maksimum {MaxLength} simvol ola bilər.");
    }
}