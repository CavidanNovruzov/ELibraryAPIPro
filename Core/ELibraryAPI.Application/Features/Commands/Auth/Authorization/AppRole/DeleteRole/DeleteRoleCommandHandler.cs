using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.DeleteRole;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.DeleteRole
{
    public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public DeleteRoleCommandHandler(IUnitOfWork uow, ICacheService cacheService)
        {
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(DeleteRoleCommandRequest request, CancellationToken ct)
        {
            var writeRepository = _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();
            var success = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>().ExistsAsync(r => r.Id == request.Id, false, ct);

            if (!success) return Result.Failure("Rol tapılmadı.");

            await writeRepository.RemoveAsync(request.Id, ct);

            var isDeleted = await _uow.SaveAsync(ct) > 0;
            if (isDeleted)
            {
                await _cacheService.RemoveAsync("roles:all", ct);
                await _cacheService.RemoveAsync($"role:detail:{request.Id}", ct);

                return Result.Success("Rol uğurla silindi.");
            }

            return Result.Failure("Rol silinərkən xəta baş verdi.");
        }
    }
}
