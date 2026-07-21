using ELibraryAPI.Application.Features.Queries.Campaign.GetAllCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class GetAllCampaignQueryValidator : AbstractValidator<GetAllCampaignQueryRequest>
{
    public GetAllCampaignQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Səhifə nömrəsi ən azı {ComparisonValue} olmalıdır.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Siyahı ölçüsü {From} ilə {To} arasında olmalıdır.");
    }
}