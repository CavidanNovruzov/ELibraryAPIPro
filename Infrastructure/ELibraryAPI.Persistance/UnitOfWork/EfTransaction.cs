using ELibraryAPI.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace ELibraryAPI.Persistence.UnitOfWork;

public sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _innerTransaction;

    public EfTransaction(IDbContextTransaction innerTransaction)
    {
        _innerTransaction = innerTransaction;
    }

    public Task CommitAsync(CancellationToken ct = default)
    {
        return _innerTransaction.CommitAsync(ct);
    }

    public Task RollbackAsync(CancellationToken ct = default)
    {
        return _innerTransaction.RollbackAsync(ct);
    }

    public ValueTask DisposeAsync()
    {
        return _innerTransaction.DisposeAsync();
    }
}