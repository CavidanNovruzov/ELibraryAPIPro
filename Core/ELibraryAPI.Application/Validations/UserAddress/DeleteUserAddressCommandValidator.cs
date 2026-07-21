using ELibraryAPI.Application.Features.Commands.UserAddress.DeleteUserAddress;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserAddress;

public sealed class DeleteUserAddressCommandValidator : AbstractValidator<DeleteUserAddressCommandRequest>
{
    public DeleteUserAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
