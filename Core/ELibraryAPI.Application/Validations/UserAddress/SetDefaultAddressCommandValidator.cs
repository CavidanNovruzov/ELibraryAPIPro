using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.SetDefaultAddress;

public sealed class SetDefaultAddressCommandValidator : AbstractValidator<SetDefaultAddressCommandRequest>
{
    public SetDefaultAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Ünvan ID-si mütləqdir.");
    }
}