using ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.InventoryMovement
{
    public sealed class GetAllInventoryMovementQueryValidator : AbstractValidator<GetAllInventoryMovementQueryRequest>
    {
        public GetAllInventoryMovementQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page {ComparisonValue}-dan böyük olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
