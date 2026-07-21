using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Domain.Entities.Common;


namespace ELibraryAPI.Application.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// RefreshToken üçün xüsusi repository — generic UoW pattern-ə əlavə olaraq saxlanılır.
    /// </summary>
    IRefreshTokenReadRepository  RefreshTokenRead  { get; }
    IRefreshTokenWriteRepository RefreshTokenWrite { get; }

    IReadRepository<T, TKey>  ReadRepository<T, TKey>()
        where T : class, IEntity<TKey>;

    IWriteRepository<T, TKey> WriteRepository<T, TKey>()
        where T : class, IEntity<TKey>;

    Task<int> SaveAsync(CancellationToken ct = default);

    Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default);
}
