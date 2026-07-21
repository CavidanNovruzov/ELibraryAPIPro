using ELibraryAPI.Application.Abstractions.Repositories.Entities;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;

namespace ELibraryAPI.Persistance.Concrete.Repositories.Entities;

public class BranchWorkHoursWriteRepository(ELibraryDbContext context) : WriteRepository<BranchWorkHours, Guid>(context), IBranchWorkHoursWriteRepository { }
