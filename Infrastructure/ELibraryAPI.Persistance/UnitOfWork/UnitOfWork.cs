using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Persistance.Concrete.Repositories;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ELibraryAPI.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ELibraryDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<(Type EntityType, bool IsRead), object> _repositories = new();

    public UnitOfWork(ELibraryDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IRefreshTokenReadRepository RefreshTokenRead => _serviceProvider.GetRequiredService<IRefreshTokenReadRepository>();
    public IRefreshTokenWriteRepository RefreshTokenWrite => _serviceProvider.GetRequiredService<IRefreshTokenWriteRepository>();
    public IPermissionReadRepository PermissionRead => _serviceProvider.GetRequiredService<IPermissionReadRepository>();
    public IPermissionWriteRepository PermissionWrite => _serviceProvider.GetRequiredService<IPermissionWriteRepository>();
    public IRolePermissionReadRepository RolePermissionRead => _serviceProvider.GetRequiredService<IRolePermissionReadRepository>();
    public IRolePermissionWriteRepository RolePermissionWrite => _serviceProvider.GetRequiredService<IRolePermissionWriteRepository>();
    public IUserPermissionReadRepository UserPermissionRead => _serviceProvider.GetRequiredService<IUserPermissionReadRepository>();
    public IUserPermissionWriteRepository UserPermissionWrite => _serviceProvider.GetRequiredService<IUserPermissionWriteRepository>();

    public IReadRepository<T, TKey> ReadRepository<T, TKey>()
        where T : class, IEntity<TKey>
    {
        var key = (typeof(T), isRead: true);

        return (IReadRepository<T, TKey>)_repositories.GetOrAdd(key, _ =>
            _serviceProvider.GetRequiredService<IReadRepository<T, TKey>>());
    }

    public IWriteRepository<T, TKey> WriteRepository<T, TKey>()
        where T : class, IEntity<TKey>
    {
        var key = (typeof(T), isRead: false);

        return (IWriteRepository<T, TKey>)_repositories.GetOrAdd(key, _ =>
            _serviceProvider.GetRequiredService<IWriteRepository<T, TKey>>());
    }

    public async Task<int> SaveAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_context.Database.CurrentTransaction != null)
            return new EfTransaction(_context.Database.CurrentTransaction);

        return new EfTransaction(await _context.Database.BeginTransactionAsync(ct));
    }

    public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();
}