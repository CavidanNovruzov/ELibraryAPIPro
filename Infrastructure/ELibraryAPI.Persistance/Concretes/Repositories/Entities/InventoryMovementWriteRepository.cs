using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class InventoryMovementWriteRepository(ELibraryDbContext context) : WriteRepository<InventoryMovement, Guid>(context), IInventoryMovementWriteRepository { }
