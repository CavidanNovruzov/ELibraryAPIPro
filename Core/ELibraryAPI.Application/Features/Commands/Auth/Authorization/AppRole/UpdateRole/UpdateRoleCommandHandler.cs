using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public UpdateRoleCommandHandler(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(UpdateRoleCommandRequest request, CancellationToken ct)
    {
        var readRepository = _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();
        var role = await readRepository.GetByIdAsync(request.Id, true, ct);

        if (role == null)
        {
            return Result.Failure("Rol tapılmadı.");
        }

        var isNameExists = await readRepository.ExistsAsync(r => r.Name == request.Name && r.Id != request.Id, false, ct);
        if (isNameExists)
        {
            return Result.Conflict("Another role with this name artıq mövcuddur.");
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();

        _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>().Update(role);

        var isSuccess = await _uow.SaveAsync(ct) > 0;
        if (isSuccess)
        {
            await _cacheService.RemoveAsync("roles:all", ct);
            await _cacheService.RemoveAsync($"role:detail:{request.Id}", ct);

            return Result.Success("Rol uğurla yeniləndi.");
        }

        return Result.Failure("Heç bir dəyişiklik yadda saxlanılmadı.");
    }
}