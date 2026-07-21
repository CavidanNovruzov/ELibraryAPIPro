using ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommandRequest>
{
    public CreateAuthorCommandValidator()
    {
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

        RuleFor(x => x.ImagePath)
            .MaximumLength(1000)
            .WithMessage("ImagePath maksimum {MaxLength} simvol ola bilər.");
    }
}