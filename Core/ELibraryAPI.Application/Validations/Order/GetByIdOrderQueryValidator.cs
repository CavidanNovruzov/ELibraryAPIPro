using ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Order;

public sealed class GetByIdOrderQueryValidator : AbstractValidator<GetByIdOrderQueryRequest>
{
    public GetByIdOrderQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sifariş ID-si mütləqdir.")
            .NotEqual(Guid.Empty).WithMessage("Düzgün Sifariş ID-si mütləqdir.");
    }
}
