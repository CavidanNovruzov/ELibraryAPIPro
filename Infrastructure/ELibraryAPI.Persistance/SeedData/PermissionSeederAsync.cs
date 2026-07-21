using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ELibraryAPI.Persistance.Data;

public static class PermissionSeeder
{
    public static async Task SeedPermissionsAsync(ELibraryDbContext context)
    {
        var permissionKeys = typeof(AuthorizePermissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Select(f => f.GetValue(null)?.ToString())
            .Where(v => v != null)
            .Distinct()
            .ToList();


        var existingPermissions = await context.Permissions
            .IgnoreQueryFilters()
            .Select(p => p.Key)
            .ToListAsync();

        var newPermissions = new List<Permission>();

        foreach (var key in permissionKeys)
        {
            if (!existingPermissions.Contains(key!))
            {
                newPermissions.Add(new Permission
                {
                    Key = key!,
                    IsDelegatable = true,
                    CreatedDate = DateTime.UtcNow
                });
            }
        }

        if (newPermissions.Count != 0)
        {
            await context.Permissions.AddRangeAsync(newPermissions);
            await context.SaveChangesAsync();
        }
    }
}
