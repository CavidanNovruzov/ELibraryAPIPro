using ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Author;

public sealed class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommandRequest>
{
    public CreateAuthorCommandValidator()
    {
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

        RuleFor(x => x.ImagePath)
            .MaximumLength(1000)
            .WithMessage("Şəkil yolu maksimum {MaxLength} simvol ola bilər.");
    }
}