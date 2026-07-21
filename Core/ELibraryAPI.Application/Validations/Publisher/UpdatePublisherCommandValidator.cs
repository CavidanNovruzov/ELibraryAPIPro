using ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommandRequest>
{
    public UpdatePublisherCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Publisher name is required.")
            .MaximumLength(200).WithMessage("Publisher name cannot exceed 200 characters.")
            .MinimumLength(2).WithMessage("Publisher name must be at least 2 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
