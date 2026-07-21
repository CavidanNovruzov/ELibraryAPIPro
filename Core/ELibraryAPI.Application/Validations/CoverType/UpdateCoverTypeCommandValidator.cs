using ELibraryAPI.Application.Features.Commands.CoverType.UpdateCoverType;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.CoverType;

public sealed class UpdateCoverTypeCommandValidator : AbstractValidator<UpdateCoverTypeCommandRequest>
{
    public UpdateCoverTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Üz qabığı növünün ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz ID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Üz qabığı növünün adı boş ola bilməz.")
            .MaximumLength(50).WithMessage("Üz qabığı növünün adı maksimum {MaxLength} simvol ola bilər.");
    }
}