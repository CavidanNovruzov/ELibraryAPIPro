using ELibraryAPI.Application.Features.Commands.Author.UpdateAuthor;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommandRequest>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id boş ola bilməz.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("FullName boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("FullName maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("Country maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Biography)
            .NotEmpty()
            .WithMessage("Biography boş ola bilməz.");
    }
}