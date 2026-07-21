using Azure.Storage.Blobs;
using ELibraryAPI.Application.Abstractions.Services.Storage;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ELibraryAPI.Infrastructure.Services.Storage;

public sealed class AzureBlobStorage : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobStorage(IConfiguration configuration)
    {
        var connStr = configuration["AzureStorage:ConnectionString"]!;
        _containerName = configuration["AzureStorage:ContainerName"] ?? "elibrary-uploads";
        _blobServiceClient = new BlobServiceClient(connStr);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string path)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobName = $"{path}/{fileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public void Delete(string path, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        containerClient.GetBlobClient($"{path}/{fileName}").DeleteIfExists();
    }

    public bool HasFile(string path, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        return containerClient.GetBlobClient($"{path}/{fileName}").Exists();
    }
}