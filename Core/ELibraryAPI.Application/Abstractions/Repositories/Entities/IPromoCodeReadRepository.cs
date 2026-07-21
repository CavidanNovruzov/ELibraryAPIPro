using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Domain.Entities.Concrete;

namespace ELibraryAPI.Application.Abstractions.Repositories.Entities;

// Marketinq və Satış
public interface IPromoCodeReadRepository : IReadRepository<PromoCode,Guid> { }
