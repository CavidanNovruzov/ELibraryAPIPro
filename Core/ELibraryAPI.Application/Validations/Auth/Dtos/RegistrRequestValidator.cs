using ELibraryAPI.Application.Dtos.Auth;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.Dtos;

public sealed class RegistrRequestValidator : AbstractValidator<RegistrRequest>
{
    public RegistrRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Ad boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("Ad maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Soyad boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("Soyad maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("İstifadəçi adı boş ola bilməz.")
            .MinimumLength(3)
            .WithMessage("İstifadəçi adı minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(50)
            .WithMessage("İstifadəçi adı maksimum {MaxLength} simvol ola bilər.")
            .Matches("^[a-zA-Z0-9._-]+$")
            .WithMessage("UserName yalnız hərflər, rəqəmlər, nöqtə, alt-xətt və defisdən ibarət ola bilər.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email boş ola bilməz.")
            .EmailAddress()
            .WithMessage("Düzgün bir email ünvanı daxil edin.")
            .MaximumLength(256)
            .WithMessage("E-poçt maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(8)
            .WithMessage("Şifrə minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(128)
            .WithMessage("Şifrə maksimum {MaxLength} simvol ola bilər.");
    }
}