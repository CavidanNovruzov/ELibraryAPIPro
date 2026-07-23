using ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BranchWorkHours;

public sealed class CreateBranchWorkHoursCommandValidator : AbstractValidator<CreateBranchWorkHoursCommandRequest>
{
    public CreateBranchWorkHoursCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Filial seçilməlidir.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz Filial ID-si.");

        RuleFor(x => x.Day)
            .IsInEnum()
            .WithMessage("Düzgün həftə günü seçin.");

        RuleFor(x => x.OpenTime)
            .NotEmpty()
            .WithMessage("Açılış vaxtı tələb olunur.");

        RuleFor(x => x.CloseTime)
            .NotEmpty()
            .WithMessage("Bağlanış vaxtı tələb olunur.");

        RuleFor(x => x)
            .Must(x => x.CloseTime > x.OpenTime)
            .WithMessage("Bağlanış vaxtı açılış vaxtından böyük olmalıdır.")
            .When(x => x.OpenTime != default && x.CloseTime != default);

        RuleFor(x => x.OpenTime)
            .Must(t => t.TotalHours >= 0 && t.TotalHours < 24)
            .WithMessage("Açılış vaxtı 00:00 ilə 23:59 arasında olmalıdır.");

        RuleFor(x => x.CloseTime)
            .Must(t => t.TotalHours >= 0 && t.TotalHours < 24)
            .WithMessage("Bağlanış vaxtı 00:00 ilə 23:59 arasında olmalıdır.");
    }
}