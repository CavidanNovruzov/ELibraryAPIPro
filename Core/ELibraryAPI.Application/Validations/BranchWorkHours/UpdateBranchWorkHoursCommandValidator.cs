using ELibraryAPI.Application.Features.Commands.BranchWorkHours.UpdateBranchWorkHours;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BranchWorkHours;

public sealed class UpdateBranchWorkHoursCommandValidator : AbstractValidator<UpdateBranchWorkHoursCommandRequest>
{
    public UpdateBranchWorkHoursCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("İş saatı ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz ID.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Filial ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz Filial ID.");

        RuleFor(x => x.Day)
            .IsInEnum().WithMessage("Düzgün həftə günü seçilməlidir.");

        RuleFor(x => x.OpenTime)
            .NotEmpty().WithMessage("Açılış vaxtı tələb olunur.");

        RuleFor(x => x.CloseTime)
            .NotEmpty().WithMessage("Bağlanış vaxtı tələb olunur.")
            .GreaterThan(x => x.OpenTime).WithMessage("Bağlanış vaxtı açılış vaxtından sonrakı bir vaxt olmalıdır.");
    }
}