namespace ELibraryAPI.Application.Abstractions.Services.Caching;

/// <summary>
/// Bu interfeysi implement edən hər Query request avtomatik olaraq
/// CachingBehavior tərəfindən cache-lənəcək.
/// </summary>
public interface ICacheable
{
    /// <summary>Cache açarı — unikal olmalıdır. Məs: "products:list:page1:size20"</summary>
    string CacheKey { get; }

    /// <summary>
    /// Mütləq silinmə müddəti (Absolute Expiration).
    /// Default olaraq 5 dəqiqə təyin edirik.
    /// </summary>
    TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(5);

    /// <summary>
    /// Sürüşkən silinmə müddəti (Sliding Expiration).
    /// Hər müraciətdə ömrü bu qədər uzadılacaq. Default olaraq 1 dəqiqə.
    /// </summary>
    TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(1);
}