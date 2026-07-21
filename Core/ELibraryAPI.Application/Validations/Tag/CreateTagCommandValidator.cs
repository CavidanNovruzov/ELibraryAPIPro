using FluentValidation;

namespace ELibraryAPI.Application.Features.Commands.Tag.CreateTag;

public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommandRequest>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name cannot be empty.")
            .MaximumLength(50).WithMessage("Tag name cannot exceed 50 characters.");
    }
}