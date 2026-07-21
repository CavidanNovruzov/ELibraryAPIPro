using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;


namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class AuthorReadRepository(ELibraryDbContext context) : ReadRepository<Author, Guid>(context), IAuthorReadRepository { }   

