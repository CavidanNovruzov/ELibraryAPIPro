using ELibraryAPI.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ELibraryAPI.Application.Abstractions.Repositories;

public interface IReadRepository<T, TKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    IQueryable<T> GetAll(bool tracking = false);

    IQueryable<T> GetWhere(
        Expression<Func<T, bool>> method,
        bool tracking = false);

    Task<T?> GetSingleAsync(
        Expression<Func<T, bool>> method,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes);

    Task<T?> GetByIdAsync(
        TKey id,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// OrderBy parametrli pagination — deterministik nəticə zəmanəti.
    /// OrderBy olmadan SQL Server hər dəfə fərqli sıralama verə bilər.
    /// </summary>
    IQueryable<T> GetPaginated<TOrderKey>(
        int page,
        int size,
        Expression<Func<T, TOrderKey>> orderBy,
        bool descending = false,
        bool tracking = false);

    /// <summary>
    /// Köhnə imza — geriyə uyğunluq üçün saxlanılır.
    /// Yeni kodda GetPaginated&lt;TOrderKey&gt; versiyasını istifadə edin.
    /// </summary>
    IQueryable<T> GetPaginated(int page, int size, bool tracking = false);

    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        CancellationToken ct = default);
}
