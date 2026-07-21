using ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangePassword;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.AppUser
{
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommandRequest>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("İstifadəçi ID-si boş ola bilməz.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Cari şifrə boş ola bilməz.")
                .Length(8, 128)
                .WithMessage("Cari şifrə ən azı {MinLength}, ən çoxu {MaxLength} simvoldan ibarət olmalıdır.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Yeni şifrə boş ola bilməz.")
                .Length(8, 128)
                .WithMessage("Yeni şifrə ən azı {MinLength}, ən çoxu {MaxLength} simvoldan ibarət olmalıdır.")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("Yeni şifrə cari şifrə ilə eyni ola bilməz.");
        }
    }
}