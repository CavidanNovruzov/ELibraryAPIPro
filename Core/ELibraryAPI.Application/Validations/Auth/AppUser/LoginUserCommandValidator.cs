using ELibraryAPI.Application.Features.Commands.Auth.LoginUser;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommandRequest>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Daxilolma adı (Login) boş ola bilməz.")
            .MaximumLength(256)
            .WithMessage("Daxilolma adı (Login) maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(8)
            .WithMessage("Şifrə minimum {MinLength} simvol olmalıdır.")
            .MaximumLength(128)
            .WithMessage("Şifrə maksimum {MaxLength} simvol ola bilər.");
    }
}