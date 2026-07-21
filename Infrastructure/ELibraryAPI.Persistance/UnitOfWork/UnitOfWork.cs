using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Common;
using ELibraryAPI.Persistance.Concrete.Repositories;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ELibraryAPI.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ELibraryDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<string, object> _repositories = new();

    private IRefreshTokenReadRepository?    _refreshTokenRead;
    private IRefreshTokenWriteRepository?   _refreshTokenWrite;
    private IPermissionReadRepository?      _permissionRead;
    private IPermissionWriteRepository?     _permissionWrite;
    private IRolePermissionReadRepository?  _rolePermissionRead;
    private IRolePermissionWriteRepository? _rolePermissionWrite;
    private IUserPermissionReadRepository?  _userPermissionRead;
    private IUserPermissionWriteRepository? _userPermissionWrite;

    public UnitOfWork(ELibraryDbContext context, IServiceProvider serviceProvider)
    {
        _context         = context;
        _serviceProvider = serviceProvider;
    }

    public IRefreshTokenReadRepository RefreshTokenRead =>
        _refreshTokenRead ??= _serviceProvider.GetRequiredService<IRefreshTokenReadRepository>();

    public IRefreshTokenWriteRepository RefreshTokenWrite =>
        _refreshTokenWrite ??= _serviceProvider.GetRequiredService<IRefreshTokenWriteRepository>();

    public IPermissionReadRepository PermissionRead =>
        _permissionRead ??= _serviceProvider.GetRequiredService<IPermissionReadRepository>();

    public IPermissionWriteRepository PermissionWrite =>
        _permissionWrite ??= _serviceProvider.GetRequiredService<IPermissionWriteRepository>();

    public IRolePermissionReadRepository RolePermissionRead =>
        _rolePermissionRead ??= _serviceProvider.GetRequiredService<IRolePermissionReadRepository>();

    public IRolePermissionWriteRepository RolePermissionWrite =>
        _rolePermissionWrite ??= _serviceProvider.GetRequiredService<IRolePermissionWriteRepository>();

    public IUserPermissionReadRepository UserPermissionRead =>
        _userPermissionRead ??= _serviceProvider.GetRequiredService<IUserPermissionReadRepository>();

    public IUserPermissionWriteRepository UserPermissionWrite =>
        _userPermissionWrite ??= _serviceProvider.GetRequiredService<IUserPermissionWriteRepository>();

    public IReadRepository<T, TKey> ReadRepository<T, TKey>()
        where T : class, IEntity<TKey>
    {
        var key = typeof(T).Name + "Read";

        if (!_repositories.ContainsKey(key))
        {
            var custom = _serviceProvider.GetService<IReadRepository<T, TKey>>();
            _repositories[key] = custom ?? new ReadRepository<T, TKey>(_context);
        }

        return (IReadRepository<T, TKey>)_repositories[key]!;
    }

    public IWriteRepository<T, TKey> WriteRepository<T, TKey>()
        where T : class, IEntity<TKey>
    {
        var key = typeof(T).Name + "Write";

        if (!_repositories.ContainsKey(key))
        {
            var custom = _serviceProvider.GetService<IWriteRepository<T, TKey>>();
            _repositories[key] = custom ?? new WriteRepository<T, TKey>(_context);
        }

        return (IWriteRepository<T, TKey>)_repositories[key]!;
    }


    public async Task<int> SaveAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
        => new EfTransaction(await _context.Database.BeginTransactionAsync(ct));

    public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();
}
