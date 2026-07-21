
namespace ELibraryAPI.Application.Abstractions.Services.Storage
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string path);
        void Delete(string path, string fileName);

        bool HasFile(string path, string fileName);
    }
}
