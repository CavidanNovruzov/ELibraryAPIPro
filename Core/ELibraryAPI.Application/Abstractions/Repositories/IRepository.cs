using ELibraryAPI.Domain.Entities.Common;

namespace ELibraryAPI.Application.Abstractions.Repositories;

public interface IRepository<T, TKey> where T : class, IEntity<TKey>
{
}
