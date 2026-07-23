using ELibraryAPI.Application.Features.Commands.Branch.UpdateBranch;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Branch;

public sealed class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommandRequest>
{
    public UpdateBranchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Filial ID-si boş ola bilməz.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Filial adı boş ola bilməz.")
            .MaximumLength(150)
            .WithMessage("Filial adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Məkan (Ünvan) boş ola bilməz.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Telefon nömrəsi boş ola bilməz.")
            .Matches(@"^\+?[0-9\s\-]{7,20}$")
            .WithMessage("Düzgün telefon nömrəsi formatı daxil edin.");
    }
}