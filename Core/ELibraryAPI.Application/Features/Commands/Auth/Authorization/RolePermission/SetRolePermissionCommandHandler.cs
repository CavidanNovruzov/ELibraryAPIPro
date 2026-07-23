using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.RolePermission;

public sealed class SetRolePermissionsCommandHandler : IRequestHandler<SetRolePermissionsCommandRequest, Result>
{
    private readonly IUnitOfWork _uow;

    public SetRolePermissionsCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result> Handle(SetRolePermissionsCommandRequest request, CancellationToken ct)
    {
        var rolePermissionWriteRepo = _uow.WriteRepository<Domain.Entities.Concrete.Auth.RolePermission, Guid>();
        var rolePermissionReadRepo = _uow.ReadRepository<Domain.Entities.Concrete.Auth.RolePermission, Guid>();

        var existing = await rolePermissionReadRepo.GetAll(tracking: true)
            .Where(rp => rp.RoleId == request.RoleId)
            .ToListAsync(ct);

        if (existing.Any())
            rolePermissionWriteRepo.RemoveRange(existing);

        var newPermissions = request.PermissionIds.Select(pId => new Domain.Entities.Concrete.Auth.RolePermission
        {
            RoleId = request.RoleId,
            PermissionId = pId
        }).ToList();

        await rolePermissionWriteRepo.AddRangeAsync(newPermissions, ct);

        var result = await _uow.SaveAsync(ct) > 0;

        if (result)
            return Result.Success("Rol icazələri uğurla yeniləndi.");

        return Result.Failure("Heç bir dəyişiklik edilmədi və ya xəta baş verdi.");
    }
}