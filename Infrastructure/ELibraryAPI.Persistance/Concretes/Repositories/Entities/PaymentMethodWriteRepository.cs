using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class PaymentMethodWriteRepository(ELibraryDbContext context) : WriteRepository<PaymentMethod, Guid>(context), IPaymentMethodWriteRepository { }
