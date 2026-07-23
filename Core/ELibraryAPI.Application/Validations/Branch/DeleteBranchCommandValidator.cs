using ELibraryAPI.Application.Features.Commands.Branch.DeleteBranch;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Branch;

public sealed class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommandRequest>
{
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Filial ID-si boş ola bilməz.");
    }
}