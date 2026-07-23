using ELibraryAPI.Application.Dtos.Auth;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Auth.Dtos;

public sealed class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("İstifadəçi ID-si boş ola bilməz.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token boş ola bilməz.");
    }
}