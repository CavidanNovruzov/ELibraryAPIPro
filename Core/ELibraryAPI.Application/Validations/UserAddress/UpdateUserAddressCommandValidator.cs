using ELibraryAPI.Application.Features.Commands.UserAddress.UpdateUserAddress;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.UserAddress;

public sealed class UpdateUserAddressCommandValidator : AbstractValidator<UpdateUserAddressCommandRequest>
{
    public UpdateUserAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.AddressLine).NotEmpty().WithMessage("Ünvan Line boş ola bilməz.").MaximumLength(1000).WithMessage("Ünvan Line maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("İstifadəçi ID-si boş ola bilməz.");
    }
}
