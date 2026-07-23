using ELibraryAPI.Application.Features.Commands.Auth.Roles.RolePermission;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Role;

public class SetRolePermissionsCommandValidator : AbstractValidator<SetRolePermissionsCommandRequest>
{
    public SetRolePermissionsCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Rol ID-si boş ola bilməz.");

        RuleFor(x => x.PermissionIds)
            .NotEmpty()
            .WithMessage("İcazə ID-ləri siyahısı boş ola bilməz.");
    }
}