using ELibraryAPI.Application.Features.Commands.Genre.UpdateGenre;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Genre;

public sealed class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommandRequest>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Janr ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərsiz Janr ID-si.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Janr adı boş ola bilməz.")
            .MaximumLength(200).WithMessage("Janr adı maksimum {MaxLength} simvol ola bilər.");
    }
}