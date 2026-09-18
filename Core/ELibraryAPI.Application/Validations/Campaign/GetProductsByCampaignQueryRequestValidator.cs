using ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public class GetProductsByCampaignQueryRequestValidator : AbstractValidator<GetProductsByCampaignQueryRequest>
{
    public GetProductsByCampaignQueryRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Size).InclusiveBetween(1, 100)
            .WithMessage("Size parametrı 1 ilə 100 arasında olmalıdır.");
    }
}
