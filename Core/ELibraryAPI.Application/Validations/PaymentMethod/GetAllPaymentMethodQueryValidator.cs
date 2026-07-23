using ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.PaymentMethod
{
    public sealed class GetAllPaymentMethodQueryValidator : AbstractValidator<GetAllPaymentMethodQueryRequest>
    {
        public GetAllPaymentMethodQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page {ComparisonValue}-dan böyük olmalıdır.");
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
