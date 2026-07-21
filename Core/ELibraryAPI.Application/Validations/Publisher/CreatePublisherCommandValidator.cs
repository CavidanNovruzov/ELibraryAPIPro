using ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommandRequest>
{
    public CreatePublisherCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
