using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class WishlistItemWriteRepository(ELibraryDbContext context) : WriteRepository<WishlistItem, Guid>(context), IWishlistItemWriteRepository { }
