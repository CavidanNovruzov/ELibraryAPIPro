using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppUserPermission;

public sealed class SetUserPermissionCommandHandler : IRequestHandler<SetUserPermissionCommandRequest, Result>
{
    private readonly IUnitOfWork _uow;

    public SetUserPermissionCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result> Handle(SetUserPermissionCommandRequest request, CancellationToken ct)
    {
        var userPermissionWriteRepo = _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppUserPermission, Guid>();
        var userPermissionReadRepo = _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppUserPermission, Guid>();

        var existing = await userPermissionReadRepo.GetAll(tracking: true)
            .Where(up => up.UserId == request.UserId)
            .ToListAsync(ct);

        if (existing.Count > 0)
            userPermissionWriteRepo.RemoveRange(existing);

        var newPermissions = request.PermissionIds.Select(pId => new Domain.Entities.Concrete.Auth.AppUserPermission
        {
            UserId = request.UserId,
            PermissionId = pId
        }).ToList();

        await using var transaction = await _uow.BeginTransactionAsync(ct);

        try
        {
            if (existing.Count > 0)
                userPermissionWriteRepo.RemoveRange(existing);

            await userPermissionWriteRepo.AddRangeAsync(newPermissions, ct); 
            await _uow.SaveAsync(ct);
            await transaction.CommitAsync(ct);
            return Result.Success("User permissions updated successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}