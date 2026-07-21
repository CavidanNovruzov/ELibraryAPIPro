using ELibraryAPI.Application.Features.Queries.InventoryMovement.GetMovementsByProduct;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.InventoryMovement;

public sealed class GetMovementsByProductQueryValidator : AbstractValidator<GetMovementsByProductQueryRequest>
{
    public GetMovementsByProductQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
