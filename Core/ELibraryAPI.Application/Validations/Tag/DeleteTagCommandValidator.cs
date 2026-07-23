using ELibraryAPI.Application.Features.Commands.Tag.DeleteTag;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Tag;

public sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommandRequest>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");
    }
}
