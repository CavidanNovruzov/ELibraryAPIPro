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
            .WithMessage("Ad və soyad boş ola bilməz.")
            .MaximumLength(200)
            .WithMessage("Ad və soyad maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Ölkə boş ola bilməz.")
            .MaximumLength(100)
            .WithMessage("Ölkə adı maksimum {MaxLength} simvol ola bilər.");

        RuleFor(x => x.Biography)
            .NotEmpty()
            .WithMessage("Bioqrafiya boş ola bilməz.");
    }
}