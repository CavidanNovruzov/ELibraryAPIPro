using ELibraryAPI.Application.Features.Commands.Product.UploadProductImage;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Product;

public sealed class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommandRequest>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Məhsul ID-si mütləq qeyd edilməlidir.");

        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("Ən azı bir şəkil yüklənməlidir.")
            .Must(files => files != null && files.Count > 0).WithMessage("Fayl siyahısı boş ola bilməz.")
            .ForEach(fileRule =>
            {
                fileRule.Must(file => file.Content != null && file.Content.Length < 5 * 1024 * 1024)
                    .WithMessage("Şəklin ölçüsü 5 MB-dan çox ola bilməz.");

                fileRule.Must(file => !string.IsNullOrEmpty(file.FileName) &&
                                      _allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                    .WithMessage("Yalnız .jpg, .jpeg, .png və .webp formatlarına icazə verilir.");
            });
    }
}