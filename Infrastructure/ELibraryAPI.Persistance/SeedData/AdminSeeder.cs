using ELibraryAPI.Application.Options;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ELibraryAPI.Persistance.Data;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ELibraryDbContext context,
        IOptions<SeedOptions> seedOptions)
    {
        var seed = seedOptions.Value.Admin;
        if (string.IsNullOrWhiteSpace(seed?.AdminEmail)) return;

        string[] roles = { RoleNames.Admin, RoleNames.User, RoleNames.Moderator };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole { Name = roleName });
            }
        }

        // Admin roluna bütün permission-ların verilməsi
        var adminRole = await roleManager.FindByNameAsync(RoleNames.Admin);
        if (adminRole != null)
        {
            var allPermissionIds = await context.Permissions.Select(p => p.Id).ToListAsync();
            await AssignPermissionsToRoleAsync(context, adminRole.Id, allPermissionIds);
        }

        // Moderator roluna xüsusi permission-ların verilməsi
        var moderatorRole = await roleManager.FindByNameAsync(RoleNames.Moderator);
        if (moderatorRole != null)
        {
            var moderatorPermissions = await context.Permissions
                .Where(p => p.Key == AuthorizePermissions.Reviews.View ||
                            p.Key == AuthorizePermissions.Reviews.Moderate ||
                            p.Key == AuthorizePermissions.Books.View)
                .Select(p => p.Id)
                .ToListAsync();

            await AssignPermissionsToRoleAsync(context, moderatorRole.Id, moderatorPermissions);
        }

        // User (Müştəri) roluna əsas istifadəçi permission-larının verilməsi
        var userRole = await roleManager.FindByNameAsync(RoleNames.User);
        if (userRole != null)
        {
            var userPermissions = await context.Permissions
                .Where(p => p.Key == AuthorizePermissions.Books.View ||
                            p.Key == AuthorizePermissions.Authors.View ||
                            p.Key == AuthorizePermissions.Orders.View ||
                            p.Key == AuthorizePermissions.Orders.Cancel ||
                            p.Key == AuthorizePermissions.Reviews.View)
                .Select(p => p.Id)
                .ToListAsync();

            await AssignPermissionsToRoleAsync(context, userRole.Id, userPermissions);
        }

        // Super Admin istifadəçisinin yaradılması
        var adminUser = await userManager.FindByEmailAsync(seed.AdminEmail);
        if (adminUser == null)
        {
            var nameParts = (seed.AdminFullName ?? "System Admin").Split(' ', 2);

            adminUser = new AppUser
            {
                UserName = seed.AdminEmail.Split('@')[0],
                Email = seed.AdminEmail,
                FirstName = nameParts[0],
                LastName = nameParts.Length > 1 ? nameParts[1] : "Admin",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, seed.AdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
            }
        }
    }

    private static async Task AssignPermissionsToRoleAsync(
        ELibraryDbContext context,
        Guid roleId,
        List<int> permissionIds)
    {
        var existingRolePermissionIds = await context.RolePermissions
            .IgnoreQueryFilters()
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

        var permissionsToAssign = permissionIds
            .Except(existingRolePermissionIds)
            .Select(pId => new RolePermission
            {
                RoleId = roleId,
                PermissionId = pId
            }).ToList();

        if (permissionsToAssign.Any())
        {
            await context.RolePermissions.AddRangeAsync(permissionsToAssign);
            await context.SaveChangesAsync();
        }
    }
}