using ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.OrderStatus
{
    public sealed class GetAllOrderStatusQueryValidator : AbstractValidator<GetAllOrderStatusQueryRequest>
    {
        public GetAllOrderStatusQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page {ComparisonValue}-dan böyük olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
