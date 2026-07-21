using ELibraryAPI.Application.Features.Commands.Auth.LoginUser;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommandRequest>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Login boş ola bilməz.")
            .MaximumLength(256)
            .WithMessage("Login maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password boş ola bilməz.")
            .MinimumLength(8)
            .WithMessage("Password minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(128)
            .WithMessage("Password maksimum {MaxLength} simvol ola bilər.");
    }
}