using ELibraryAPI.Application.Features.Commands.Campaign.CreateCampaign;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Campaign;

public sealed class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommandRequest>
{
    public CreateCampaignCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Kampaniya adı boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("Kampaniya adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.DiscountPercent)
            .InclusiveBetween(0, 100)
            .WithMessage("Endirim faizi {From} ilə {To} arasında olmalıdır.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Başlanğıc tarixi keçmiş tarix ola bilməz.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("Bitmə tarixi başlanğıc tarixindən sonra olmalıdır.");
    }
}