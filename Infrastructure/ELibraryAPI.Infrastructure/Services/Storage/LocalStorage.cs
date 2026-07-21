using Microsoft.AspNetCore.Hosting;
using ELibraryAPI.Application.Abstractions.Services.Storage;

namespace ELibraryAPI.Infrastructure.Services.Storage;

public class LocalStorage : IStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string path)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        string fullPath = Path.Combine(uploadPath, fileName);

        using (var targetStream = new FileStream(
            fullPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true))
        {
            await fileStream.CopyToAsync(targetStream);
        }

        return $"/uploads/{path}/{fileName}";
    }

    public void Delete(string path, string fileName)
    {
        string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path, fileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    public bool HasFile(string path, string fileName)
        => File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path, fileName));
}