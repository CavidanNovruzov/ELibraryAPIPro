using ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommandRequest>
{
    public CreatePublisherCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("təsviri boş ola bilməz.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("adı boş ola bilməz.").MaximumLength(200).WithMessage("adı maksimum {MaxLength} simvol ola bilər.");
    }
}
