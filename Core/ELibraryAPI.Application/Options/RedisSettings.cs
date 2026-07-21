namespace ELibraryAPI.Application.Options;

public class RedisSettings
{
    public List<string> SentinelEndpoints { get; set; } = new();
    public string MasterName { get; set; }
    public bool IsLocalDockerDev { get; set; }
    public Dictionary<string, string> IPTranslations { get; set; } = new();
}