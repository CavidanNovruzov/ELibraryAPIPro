using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.Tag.CreateTag;

public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommandRequest>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Teq adı boş ola bilməz.")
            .MaximumLength(50).WithMessage("Teq adı 50 simvoldan çox ola bilməz.");
    }
}