
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetAllRolePermission;

public sealed class GetAllRolePermissionQueryHandler : IRequestHandler<GetAllRolePermissionQueryRequest, Result<GetAllRolePermissionQueryResponse>>
{
    private readonly IUnitOfWork _uow;

    public GetAllRolePermissionQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<GetAllRolePermissionQueryResponse>> Handle(GetAllRolePermissionQueryRequest request, CancellationToken ct)
    {
        var rolePermissions = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.RolePermission, Guid>().GetAll(tracking: false)
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Select(rp => new RolePermissionListDto(
                rp.RoleId,
                rp.Role.Name ?? string.Empty,
                rp.PermissionId,
                rp.Permission.Key
            ))
            .ToListAsync(ct);

        var response = new GetAllRolePermissionQueryResponse(rolePermissions);

        return Result<GetAllRolePermissionQueryResponse>.Success(response);
    }
}
