using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateRoleCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateRoleCommandRequest request, CancellationToken ct)
    {
        var readRepository = _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();
        var role = await readRepository.GetByIdAsync(request.Id, true, ct);

        if (role == null)
        {
            return Result.Failure("Role not found.");
        }

        var isNameExists = await readRepository.ExistsAsync(r => r.Name == request.Name && r.Id != request.Id, false, ct);
        if (isNameExists)
        {
            return Result.Failure("Another role with this name already exists.");
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();

        _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>().Update(role);

        return await _uow.SaveAsync(ct) > 0
            ? Result.Success("Role updated successfully.")
            : Result.Failure("No changes were saved.");
    }
}