

namespace ELibraryAPI.Domain.Entities.Common
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
