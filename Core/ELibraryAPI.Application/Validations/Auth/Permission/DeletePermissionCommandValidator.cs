using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.DeletePermission;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Permission;

public sealed class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommandRequest>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id 0-dan böyük olmalıdır.");
    }
}