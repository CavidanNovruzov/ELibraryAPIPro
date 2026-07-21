using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class TransactionWriteRepository(ELibraryDbContext context) : WriteRepository<Transaction, Guid>(context), ITransactionWriteRepository { }
