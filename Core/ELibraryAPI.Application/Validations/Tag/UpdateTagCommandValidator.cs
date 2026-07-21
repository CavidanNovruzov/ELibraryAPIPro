using ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Tag;

public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommandRequest>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
