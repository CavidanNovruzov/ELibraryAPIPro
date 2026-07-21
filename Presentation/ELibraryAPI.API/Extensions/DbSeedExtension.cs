using ELibraryAPI.Application.Options;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistance.Data;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ELibraryAPI.API.Extensions;

public static class DbSeedExtension
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ELibraryDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var seedOptions = scope.ServiceProvider.GetRequiredService<IOptions<SeedOptions>>();

        try
        {
            await PermissionSeeder.SeedPermissionsAsync(context);
            await AdminSeeder.SeedAdminAsync(userManager, roleManager, context, seedOptions);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}