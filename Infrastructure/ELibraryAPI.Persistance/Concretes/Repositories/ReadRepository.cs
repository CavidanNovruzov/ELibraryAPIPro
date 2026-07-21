using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ELibraryAPI.Persistance.Concrete.Repositories;

public class ReadRepository<T, TKey> : IReadRepository<T, TKey>
    where T : class, IEntity<TKey>
{
    private readonly ELibraryDbContext _context;

    public ReadRepository(ELibraryDbContext context)
    {
        _context = context;
    }

    protected DbSet<T> Table => _context.Set<T>();

    public IQueryable<T> GetAll(bool tracking = false)
    {
        var query = Table.AsQueryable();
        if (!tracking) query = query.AsNoTracking();
        return query;
    }

    public async Task<T?> GetByIdAsync(
        TKey id,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Table.AsQueryable();

        if (includes is { Length: > 0 })
            foreach (var include in includes)
                query = query.Include(include);

        if (!tracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id), ct);
    }

    public async Task<T?> GetSingleAsync(
        Expression<Func<T, bool>> method,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Table.AsQueryable();

        if (includes is { Length: > 0 })
            foreach (var include in includes)
                query = query.Include(include);

        if (!tracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(method, ct);
    }

    public IQueryable<T> GetWhere(
        Expression<Func<T, bool>> method,
        bool tracking = false)
    {
        var query = Table.Where(method);
        if (!tracking) query = query.AsNoTracking();
        return query;
    }

    public IQueryable<T> GetPaginated<TOrderKey>(
        int page,
        int size,
        Expression<Func<T, TOrderKey>> orderBy,
        bool descending = false,
        bool tracking = false)
    {
        var query = Table.AsQueryable();
        if (!tracking) query = query.AsNoTracking();

        query = descending
            ? query.OrderByDescending(orderBy)
            : query.OrderBy(orderBy);

        return query.Skip((page - 1) * size).Take(size);
    }

    public IQueryable<T> GetPaginated(int page, int size, bool tracking = false)
    {
        var query = Table.AsQueryable();
        if (!tracking) query = query.AsNoTracking();

        return query
            .OrderBy(e => e.Id)     
            .Skip((page - 1) * size)
            .Take(size);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        CancellationToken ct = default)
    {
        var query = Table.AsQueryable();
        if (!tracking) query = query.AsNoTracking();
        return await query.AnyAsync(predicate, ct);
    }
}
