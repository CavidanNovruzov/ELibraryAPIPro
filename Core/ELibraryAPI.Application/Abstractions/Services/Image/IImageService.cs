namespace ELibraryAPI.Application.Abstractions.Services.Image;

public interface IImageService
{
    Task<Stream> CompressAndConvertToWebpAsync(
        Stream inputStream,
        int maxWidth = 1200,
        int maxHeight = 1200,
        int quality = 80,
        CancellationToken ct = default);
}


