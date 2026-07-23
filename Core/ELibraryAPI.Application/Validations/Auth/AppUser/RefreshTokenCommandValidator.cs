using ELibraryAPI.Application.Features.Commands.Auth.RefreshToken;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommandRequest>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Yeniləmə tokeni (RefreshToken) boş ola bilməz.")
            .MaximumLength(1024)
            .WithMessage("Yeniləmə tokeni (RefreshToken) maksimum {MaxLength} simvol ola bilər.");
    }
}