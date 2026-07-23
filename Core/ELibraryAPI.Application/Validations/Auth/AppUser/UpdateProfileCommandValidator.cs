using FluentValidation;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommandRequest>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("İstifadəçi ID-si boş ola bilməz.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Ad boş ola bilməz.")
            .MaximumLength(50)
            .WithMessage("Ad maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Soyad boş ola bilməz.")
            .MaximumLength(50)
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
    }
}