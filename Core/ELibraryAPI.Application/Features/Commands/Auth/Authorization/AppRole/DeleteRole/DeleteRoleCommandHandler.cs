using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.DeleteRole
{
    public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteRoleCommandRequest request, CancellationToken ct)
        {
            var writeRepository = _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();
            var success = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>().ExistsAsync(r => r.Id == request.Id,false, ct);

            if (!success) return Result.Failure("Role not found.");

            await writeRepository.RemoveAsync(request.Id, ct);
            return await _uow.SaveAsync(ct) > 0
                ? Result.Success("Role deleted successfully.")
                : Result.Failure("An error occurred while deleting the role.");
        }
    }
}
