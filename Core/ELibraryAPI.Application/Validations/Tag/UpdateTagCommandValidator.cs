using ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Tag;

public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommandRequest>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(100).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
    }
}
