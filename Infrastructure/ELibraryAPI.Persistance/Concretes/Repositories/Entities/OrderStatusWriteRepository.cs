using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class OrderStatusWriteRepository(ELibraryDbContext context) : WriteRepository<OrderStatus,Guid>(context), IOrderStatusWriteRepository { }
