using ELibraryAPI.Application.Features.Commands.Genre.CreateGenre;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Genre;

public sealed class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommandRequest>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Janr adı boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("Janr adı maksimum {MaxLength} simvol ola bilər.");
    }
}