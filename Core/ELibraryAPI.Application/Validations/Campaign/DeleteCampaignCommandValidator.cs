using ELibraryAPI.Application.Features.Commands.Campaign.DeleteCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class DeleteCampaignCommandValidator : AbstractValidator<DeleteCampaignCommandRequest>
{
    public DeleteCampaignCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Kampaniya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Kampaniya ID-si.");
    }
}