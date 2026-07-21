using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class TagWriteRepository(ELibraryDbContext context) : WriteRepository<Tag, Guid>(context), ITagWriteRepository { }
