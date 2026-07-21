using ELibraryAPI.Application.Features.Queries.Branch.GetByIdBranch;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Branch;

public sealed class GetByIdBranchQueryValidator : AbstractValidator<GetByIdBranchQueryRequest>
{
    public GetByIdBranchQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Filial ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Keçərli bir ID daxil edin.");
    }
}
