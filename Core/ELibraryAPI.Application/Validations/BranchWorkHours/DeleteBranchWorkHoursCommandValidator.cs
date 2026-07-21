using ELibraryAPI.Application.Features.Commands.BranchWorkHours.DeleteBranchWorkHours;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.BranchWorkHours;

public sealed class DeleteBranchWorkHoursCommandValidator : AbstractValidator<DeleteBranchWorkHoursCommandRequest>
{
    public DeleteBranchWorkHoursCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("İş saatı ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz ID.");
    }
}