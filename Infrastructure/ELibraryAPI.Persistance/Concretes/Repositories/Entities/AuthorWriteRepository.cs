using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Persistence.Contexts;


namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class AuthorWriteRepository(ELibraryDbContext context) : WriteRepository<Author, Guid>(context), IAuthorWriteRepository { } 