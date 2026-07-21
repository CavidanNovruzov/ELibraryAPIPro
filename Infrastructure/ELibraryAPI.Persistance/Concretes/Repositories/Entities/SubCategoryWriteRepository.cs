using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class SubCategoryWriteRepository(ELibraryDbContext context) : WriteRepository<SubCategory, Guid>(context), ISubCategoryWriteRepository { }
