using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.CreatePermission;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Permission;

public sealed class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommandRequest>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key boş ola bilməz.")
            .MaximumLength(150)
            .WithMessage("Key maksimum {MaxLength} simvol ola bilər.");
    }
}