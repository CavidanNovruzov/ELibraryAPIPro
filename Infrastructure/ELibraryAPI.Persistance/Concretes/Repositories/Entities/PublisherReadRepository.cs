using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class PublisherReadRepository(ELibraryDbContext context) : ReadRepository<Publisher, Guid>(context), IPublisherReadRepository { }
