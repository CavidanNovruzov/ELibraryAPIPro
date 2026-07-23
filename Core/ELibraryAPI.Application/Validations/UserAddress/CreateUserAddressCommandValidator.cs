using ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;
using FluentValidation;

public sealed class CreateUserAddressCommandValidator : AbstractValidator<CreateUserAddressCommandRequest>
{
    public CreateUserAddressCommandValidator()
    {
        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Ünvan xətti boş ola bilməz.")
            .MaximumLength(1000).WithMessage("Ünvan xətti çox uzundur.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("İstifadəçi ID-si mütləqdir.");
    }
}