using MediatR;
using ELibraryAPI.Application.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.AppRole.GetAllRoles;

public sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQueryRequest, Result<List<GetAllRolesQueryResponse>>>
{
    private readonly RoleManager<Domain.Entities.Concrete.Auth.AppRole> _roleManager;

    public GetAllRolesQueryHandler(RoleManager<Domain.Entities.Concrete.Auth.AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<List<GetAllRolesQueryResponse>>> Handle(GetAllRolesQueryRequest request, CancellationToken ct)
    {
        var roles = await _roleManager.Roles
            .AsNoTracking()
            .Select(r => new GetAllRolesQueryResponse(
                r.Id,
                r.Name!,
                r.IsActive,
                r.CreatedDate,
                r.CreatedBy
            ))
            .ToListAsync(ct);

        return Result<List<GetAllRolesQueryResponse>>.Success(roles);
    }
}