using ELibraryAPI.Application.Features.Commands.Campaign.ToggleCampaignStatus;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class ToggleCampaignStatusCommandValidator : AbstractValidator<ToggleCampaignStatusCommandRequest>
{
    public ToggleCampaignStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kampaniya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Kampaniya ID-si.");
    }
}