using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Abstractions.Repositories.Entities;

// Wishlist
public interface IWishlistReadRepository : IReadRepository<Wishlist, Guid> { }
