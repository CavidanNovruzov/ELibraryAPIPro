using System.Reflection;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ELibraryAPI.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ELibraryDbContext>
{
    public ELibraryDbContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Presentation", "ELibraryAPI.API");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets(Assembly.Load("ELibraryAPI.API"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                           $"DefaultConnection was not found! Searched path: {basePath}. " +
            "Please verify that the 'ConnectionStrings:DefaultConnection' structure exists in your secrets.json file.");
        }

        DbContextOptions<ELibraryDbContext> options = new DbContextOptionsBuilder<ELibraryDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ELibraryDbContext(options, new DesignTimeCurrentUserService());
    }
}

public sealed class DesignTimeCurrentUserService : ICurrentUserService
{
    public string? UserId => "Migration-System";
    public Guid UserGuid => Guid.Empty;
    public bool IsAuthenticated => true;
    public bool IsAdmin => false;
    public bool IsInRole(string role) => false;
}