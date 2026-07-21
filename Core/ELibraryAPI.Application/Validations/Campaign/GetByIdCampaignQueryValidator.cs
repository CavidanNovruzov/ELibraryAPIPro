using ELibraryAPI.Application.Features.Queries.Campaign.GetByIdCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class GetByIdCampaignQueryValidator : AbstractValidator<GetByIdCampaignQueryRequest>
{
    public GetByIdCampaignQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kampaniya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Kampaniya ID-si.");
    }
}