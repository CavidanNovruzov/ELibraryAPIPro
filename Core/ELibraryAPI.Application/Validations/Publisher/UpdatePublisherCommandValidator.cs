using ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommandRequest>
{
    public UpdatePublisherCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nəşriyyat adı mütləqdir.")
            .MaximumLength(200).WithMessage("Nəşriyyat adı 200 simvoldan çox ola bilməz.")
            .MinimumLength(2).WithMessage("Nəşriyyat adı ən azı 2 simvol olmalıdır.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Təsvir 1000 simvoldan çox ola bilməz.");
    }
}
