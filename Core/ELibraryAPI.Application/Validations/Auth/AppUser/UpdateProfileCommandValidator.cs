using FluentValidation;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommandRequest>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId boş ola bilməz.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName boş ola bilməz.")
            .MaximumLength(50)
            .WithMessage("FirstName maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName boş ola bilməz.")
            .MaximumLength(50)
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
    }
}