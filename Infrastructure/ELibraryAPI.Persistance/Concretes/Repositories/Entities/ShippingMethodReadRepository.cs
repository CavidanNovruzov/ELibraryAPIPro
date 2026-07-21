using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class ShippingMethodReadRepository(ELibraryDbContext context) : ReadRepository<ShippingMethod, Guid>(context), IShippingMethodReadRepository { }
