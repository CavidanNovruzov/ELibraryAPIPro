using ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Order;

public sealed class GetByIdOrderQueryValidator : AbstractValidator<GetByIdOrderQueryRequest>
{
    public GetByIdOrderQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Valid Order ID is required.");
    }
}
