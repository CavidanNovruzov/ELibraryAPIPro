using ELibraryAPI.Application.Features.Commands.UserAddress.UpdateUserAddress;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserAddress;

public sealed class UpdateUserAddressCommandValidator : AbstractValidator<UpdateUserAddressCommandRequest>
{
    public UpdateUserAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.AddressLine).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.UserId).NotEmpty();
    }
}
