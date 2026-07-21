using ELibraryAPI.Application.Features.Commands.Auth.AppUser.AssignRole;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.CreateRole;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.DeleteRole;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.UpdateRole;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.AppUserPermission;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.CreatePermission;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.DeletePermission;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.UpdatePermission;
using ELibraryAPI.Application.Features.Commands.Auth.Roles.RolePermission;
using ELibraryAPI.Application.Features.Queries.AppRole.GetAllRoles;
using ELibraryAPI.Application.Features.Queries.Auth.AppRole.GetRoleById;
using ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetAllAppUserPermission;
using ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetByIdAppUserPermission;
using ELibraryAPI.Application.Features.Queries.Auth.RolePermission.GetAllRolePermission;
using ELibraryAPI.Application.Features.Queries.Permission.GetAllPermission;
using ELibraryAPI.Application.Features.Queries.Permission.GetByIdPermission;
using ELibraryAPI.Application.Features.Queries.RolePermission.GetByIdRolePermission;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ELibraryAPI.API.Controllers.Auth.Permissions;

[Route("api/[controller]")]
[Authorize]
public sealed class SecurityController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public SecurityController(IMediator mediator) => _mediator = mediator;

    #region Permission Management (İcazələr)

    [HttpGet("permissions")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> GetAllPermissions(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllPermissionQueryRequest(), ct));

    [HttpGet("permissions/{id:int}")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> GetPermissionById([FromRoute] int id, CancellationToken ct)
    => FromResult(await _mediator.Send(new GetByIdPermissionQueryRequest(id), ct));

    [HttpPost("permissions")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("permissions/{id:int}")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> UpdatePermission([FromRoute] int id, [FromBody] UpdatePermissionCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("permissions/{id:int}")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> DeletePermission([FromRoute] int id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeletePermissionCommandRequest(id), ct));

    #endregion

    #region Role Management (Rollar)

    [HttpGet("roles")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)] 
    public async Task<IActionResult> GetAllRoles(CancellationToken ct)
    => FromResult(await _mediator.Send(new GetAllRolesQueryRequest(), ct));

    [HttpGet("roles/{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)]
    public async Task<IActionResult> GetRoleById([FromRoute] Guid id, CancellationToken ct)
    => FromResult(await _mediator.Send(new GetRoleByIdQueryRequest(id), ct));

    [HttpPost("roles")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("roles/{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)]
    public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromBody] UpdateRoleCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("roles/{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)]
    public async Task<IActionResult> DeleteRole([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteRoleCommandRequest(id), ct));

    #endregion

    #region Role-Permission Assignment (Rolların İcazələri)

    [HttpGet("roles-permissions-list")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> GetAllRolePermissions(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllRolePermissionQueryRequest(), ct));

    [HttpGet("roles/{roleId:guid}/permissions")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> GetRolePermissionsById([FromRoute] Guid roleId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdRolePermissionQueryRequest(roleId), ct));

    [HttpPost("roles/set-permissions")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> SetRolePermissions([FromBody] SetRolePermissionsCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    #endregion

    #region User Security Management (İstifadəçi Səlahiyyətləri)

    [HttpPost("users/assign-role")]
    [HasPermission(AuthorizePermissions.Administration.ManageRoles)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("users/set-custom-permission")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> SetUserCustomPermission([FromBody] SetUserPermissionCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("users/{userId:guid}/custom-permissions")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> GetUserCustomPermissions([FromRoute] Guid userId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllAppUserPermissionQueryRequest(userId), ct));

    [HttpGet("users/{userId:guid}/custom-permissions/{permissionId:int}")]
    [HasPermission(AuthorizePermissions.Administration.AssignPermissions)]
    public async Task<IActionResult> GetUserCustomPermissionById([FromRoute] Guid userId, [FromRoute] int permissionId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdAppUserPermissionQueryRequest(userId, permissionId), ct));

    #endregion
}