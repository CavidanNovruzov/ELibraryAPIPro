using ELibraryAPI.Application.Features.Commands.Branch.CreateBranch;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Branch;

public sealed class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommandRequest>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch adı boş ola bilməz.")
            .MaximumLength(150)
            .WithMessage("Branch adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location (Ünvan) boş ola bilməz.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Telefon nömrəsi boş ola bilməz.")
            .Matches(@"^\+?[0-9\s\-]{7,20}$")
            .WithMessage("Düzgün telefon nömrəsi formatı daxil edin.");
    }
}