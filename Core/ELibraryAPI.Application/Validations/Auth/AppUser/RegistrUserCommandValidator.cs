using ELibraryAPI.Application.Features.Commands.Auth.RegistrUser;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class RegistrUserCommandValidator : AbstractValidator<RegistrUserCommandRequest>
{
    public RegistrUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("FirstName maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("LastName maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName boş ola bilməz.")
            .MinimumLength(3)
            .WithMessage("UserName minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(50)
            .WithMessage("UserName maksimum {MaxLength} simvol ola bilər.")
            .Matches("^[a-zA-Z0-9._-]+$")
            .WithMessage("UserName yalnız hərflər, rəqəmlər, nöqtə, alt-xətt və defisdən ibarət ola bilər.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email boş ola bilməz.")
            .EmailAddress()
            .WithMessage("Düzgün bir email ünvanı daxil edin.")
            .MaximumLength(256)
            .WithMessage("Email maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password boş ola bilməz.")
            .MinimumLength(8)
            .WithMessage("Password minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(128)
            .WithMessage("Password maksimum {MaxLength} simvol ola bilər.");
    }
}