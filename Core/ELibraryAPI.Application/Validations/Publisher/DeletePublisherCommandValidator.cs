using ELibraryAPI.Application.Features.Commands.Publisher.DeletePublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommandRequest>
{
    public DeletePublisherCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
