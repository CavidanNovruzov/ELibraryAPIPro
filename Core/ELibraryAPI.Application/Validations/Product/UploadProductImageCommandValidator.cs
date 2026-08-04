using ELibraryAPI.Application.Features.Commands.Product.UploadProductImage;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Product;

public sealed class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommandRequest>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];


    private static readonly Dictionary<string, byte[]> _magicNumbers = new()
    {
        { ".jpg",  new byte[] { 0xFF, 0xD8, 0xFF } },
        { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
        { ".png",  new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
        { ".webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
    };

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

                fileRule.Must(file =>
                {
                    if (file.Content == null || string.IsNullOrEmpty(file.FileName))
                        return false;

                    var extension = Path.GetExtension(file.FileName).ToLower();
                    return _allowedExtensions.Contains(extension);
                }).WithMessage("Yalnız .jpg, .jpeg, .png və .webp formatlarına icazə verilir.");

                fileRule.Must(file =>
                {
                    if (file.Content == null || file.Content.Length < 4)
                        return false;

                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!_magicNumbers.TryGetValue(extension, out var expectedBytes))
                        return false;

                    var headerBytes = new byte[expectedBytes.Length];
                    var originalPosition = file.Content.Position;

                    try
                    {
                        file.Content.Position = 0;
                        file.Content.ReadExactly(headerBytes, 0, expectedBytes.Length);
                        file.Content.Position = originalPosition; 
                    }
                    catch
                    {
                        return false;
                    }

                    return headerBytes.SequenceEqual(expectedBytes);
                }).WithMessage("Faylın tərkibi şəkil formatına uyğun gəlmir (zərərli və ya zədələnmiş fayl).");
            });
    }
}