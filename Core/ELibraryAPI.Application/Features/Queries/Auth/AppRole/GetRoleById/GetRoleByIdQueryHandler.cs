using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.Auth.AppRole.GetRoleById;

public sealed class GetRoleByIdQueryHandler(IUnitOfWork uow)
: IRequestHandler<GetRoleByIdQueryRequest, Result<GetRoleByIdQueryResponse>>
{
    public async Task<Result<GetRoleByIdQueryResponse>> Handle(GetRoleByIdQueryRequest request, CancellationToken ct)
    {
        var role = await uow.ReadRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>()
            .GetByIdAsync(request.Id,
                tracking: false,
                ct: ct,
                includes: r => r.RolePermissions);

        if (role == null)
            return Result<GetRoleByIdQueryResponse>.NotFound("Role not found.");

        var permissions = role.RolePermissions
            .Select(rp => rp.PermissionId.ToString()) 
            .ToList();

        var response = new GetRoleByIdQueryResponse(
            role.Id,
            role.Name ?? string.Empty,
            permissions);

        return Result<GetRoleByIdQueryResponse>.Success(response);
    }
}
