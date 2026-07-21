
using ELibraryAPI.Application.Features.Queries.RolePermission.GetByIdRolePermission;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetByIdRolePermission;

public sealed class GetByIdRolePermissionQueryHandler : IRequestHandler<GetByIdRolePermissionQueryRequest, Result<GetByIdRolePermissionQueryResponse>>
{
    private readonly IUnitOfWork _uow;

    public GetByIdRolePermissionQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<GetByIdRolePermissionQueryResponse>> Handle(GetByIdRolePermissionQueryRequest request, CancellationToken ct)
    {
        var role = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>().GetByIdAsync(
        request.RoleId,
        tracking: false,
        ct: ct,
        includes: x => x.RolePermissions 
    );

        if (role == null)
            return Result<GetByIdRolePermissionQueryResponse>.Failure("Role not found.");

        var response = new GetByIdRolePermissionQueryResponse(
            role.Id,
            role.Name ?? string.Empty,
            role.RolePermissions.Select(rp => rp.PermissionId).ToList()
        );

        return Result<GetByIdRolePermissionQueryResponse>.Success(response);
    }
}