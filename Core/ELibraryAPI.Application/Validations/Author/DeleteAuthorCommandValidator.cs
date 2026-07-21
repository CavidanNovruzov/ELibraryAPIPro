using ELibraryAPI.Application.Features.Commands.Author.DeleteAuthor;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommandRequest>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Author ID silinmə prosesi üçün tələb olunur.");
    }
}