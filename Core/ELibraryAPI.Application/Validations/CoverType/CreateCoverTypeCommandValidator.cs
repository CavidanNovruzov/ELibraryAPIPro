using ELibraryAPI.Application.Features.Commands.CoverType.CreateCoverType;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.CoverType;

public sealed class CreateCoverTypeCommandValidator : AbstractValidator<CreateCoverTypeCommandRequest>
{
    public CreateCoverTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Üz qabığı növünün adı boş ola bilməz.")
            .MaximumLength(50)
            .WithMessage("Üz qabığı növünün adı maksimum {MaxLength} simvol ola bilər.");
    }
}