using ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.CoverType;

public sealed class GetByIdCoverTypeQueryValidator : AbstractValidator<GetByIdCoverTypeQueryRequest>
{
    public GetByIdCoverTypeQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Üz qabığı növünün ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz ID.");
    }
}