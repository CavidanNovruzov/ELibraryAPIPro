using ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;
using FluentValidation;

public sealed class CreateUserAddressCommandValidator : AbstractValidator<CreateUserAddressCommandRequest>
{
    public CreateUserAddressCommandValidator()
    {
        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Address line cannot be empty.")
            .MaximumLength(1000).WithMessage("Address line is too long.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}