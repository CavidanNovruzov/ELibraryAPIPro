using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Order
{
    public sealed class GetAllOrderQueryValidator : AbstractValidator<GetAllOrderQueryRequest>
    {
        public GetAllOrderQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
