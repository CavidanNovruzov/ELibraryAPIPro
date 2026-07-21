using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Abstractions.Repositories.Entities;

// User Detalları
public interface IUserAddressReadRepository : IReadRepository<UserAddress, Guid> { }
