using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class WishlistItemReadRepository(ELibraryDbContext context) : ReadRepository<WishlistItem, Guid>(context), IWishlistItemReadRepository { }
