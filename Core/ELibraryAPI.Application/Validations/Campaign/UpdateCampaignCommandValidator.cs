using ELibraryAPI.Application.Features.Commands.Campaign.UpdateCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class UpdateCampaignCommandValidator : AbstractValidator<UpdateCampaignCommandRequest>
{
    public UpdateCampaignCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Kampaniya ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz Kampaniya ID-si.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kampaniya adı boş ola bilməz.")
            .MaximumLength(200).WithMessage("Kampaniya adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.DiscountPercent)
            .InclusiveBetween(0, 100).WithMessage("Endirim faizi {From} ilə {To} arasında olmalıdır.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("Bitmə tarixi başlanğıc tarixindən sonra olmalıdır.");
    }
}