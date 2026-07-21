
namespace ELibraryAPI.Application.Options;

public class SeedOptions
{
    public const string SectionName = "Seed";
    public AdminSeedOptions Admin { get; set; } = new AdminSeedOptions();
}
public class AdminSeedOptions
{
    public string? AdminEmail { get; set; }
    public string? AdminPassword { get; set; }
    public string AdminFullName { get; set; } = "Admin";
}
