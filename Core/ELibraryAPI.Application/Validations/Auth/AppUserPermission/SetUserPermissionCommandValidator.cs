using FluentValidation;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.AppUserPermission;

namespace ELibraryAPI.Application.Validations.Auth.AppUser;

public class SetUserPermissionCommandValidator : AbstractValidator<SetUserPermissionCommandRequest>
{
    public SetUserPermissionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId boş ola bilməz.");

        RuleFor(x => x.PermissionIds)
            .NotEmpty()
            .WithMessage("PermissionIds boş ola bilməz.")
            .Must(p => p != null && p.Count > 0)
            .WithMessage("Permission siyahısı boş ola bilməz.");

        RuleForEach(x => x.PermissionIds)
            .GreaterThan(0)
            .WithMessage("Keçərsiz Permission ID aşkarlandı.");
    }
}