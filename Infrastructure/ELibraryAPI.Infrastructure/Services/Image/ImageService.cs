using ELibraryAPI.Application.Abstractions.Services.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;


namespace ELibraryAPI.Infrastructure.Services.Image;

public class ImageService : IImageService
{
    public async Task<Stream> CompressAndConvertToWebpAsync(
        Stream inputStream,
        int maxWidth = 1200,
        int maxHeight = 1200,
        int quality = 80,
        CancellationToken ct = default)
    {
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(inputStream, ct);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max
        }));

        var outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, new WebpEncoder { Quality = quality }, ct);
        outputStream.Position = 0;

        return outputStream;
    }
}
