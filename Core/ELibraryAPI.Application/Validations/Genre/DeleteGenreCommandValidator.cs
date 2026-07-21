using ELibraryAPI.Application.Features.Commands.Genre.DeleteGenre;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Genre;

public sealed class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommandRequest>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Janr ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty)
            .WithMessage("Keçərsiz ID.");
    }
}