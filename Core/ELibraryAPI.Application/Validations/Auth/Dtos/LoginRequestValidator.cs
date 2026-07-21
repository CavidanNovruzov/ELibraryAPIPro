using ELibraryAPI.Application.Dtos.Auth;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.Dtos;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Login boş ola bilməz.")
            .MaximumLength(256)
            .WithMessage("Login maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password boş ola bilməz.");
    }
}