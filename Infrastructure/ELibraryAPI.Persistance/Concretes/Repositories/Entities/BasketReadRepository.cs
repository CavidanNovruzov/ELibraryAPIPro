using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class BasketReadRepository(ELibraryDbContext context) : ReadRepository<Basket, Guid>(context), IBasketReadRepository { }
