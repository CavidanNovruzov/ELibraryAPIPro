using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class OrderItemWriteRepository(ELibraryDbContext context) : WriteRepository<OrderItem, Guid>(context), IOrderItemWriteRepository { }
