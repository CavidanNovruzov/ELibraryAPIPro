using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.UpdatePermission;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Permission;

public sealed class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommandRequest>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id 0-dan böyük olmalıdır.");

        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key boş ola bilməz.")
            .MaximumLength(150)
            .WithMessage("Key maksimum {MaxLength} simvol ola bilər.");
    }
}