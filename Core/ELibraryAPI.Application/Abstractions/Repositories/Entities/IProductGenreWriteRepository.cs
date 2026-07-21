using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Abstractions.Repositories.Entities;

public interface IProductGenreWriteRepository : IWriteRepository<ProductGenre, Guid> { }
