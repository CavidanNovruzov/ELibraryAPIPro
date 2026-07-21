using ELibraryAPI.API.Controllers;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangePassword;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangeUserStatus;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.DeleteUser;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;
using ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateUserByAdmin;
using ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetAllUsers;
using ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;
using ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetUserById;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Authorize]
public sealed class UsersController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    #region Personal Profile Operations (İstifadəçinin Öz Əməliyyatları)

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        if (UserId == null) return Unauthorized(); 
        return FromResult(await _mediator.Send(new GetMyProfileQueryRequest(UserId.Value), ct));
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommandRequest request, CancellationToken ct)
    {
        if (UserId == null) return Unauthorized();
        return FromResult(await _mediator.Send(request with { UserId = UserId.Value }, ct));
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandRequest request, CancellationToken ct)
    {
        if (UserId == null) return Unauthorized();
        return FromResult(await _mediator.Send(request with { UserId = UserId.Value }, ct));
    }

    #endregion

    #region Administrative Operations (Admin Əməliyyatları)

    [HttpGet]
    [HasPermission(AuthorizePermissions.Administration.ManageUsers)] 
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllUsersQueryRequest(), ct));

    [HttpGet("{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageUsers)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetUserByIdQueryRequest(id), ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageUsers)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserByAdminCommandRequest request, CancellationToken ct)
    => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManageUsers)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteUserCommandRequest(id), ct));

    [HttpPatch("{id:guid}/change-status")]
    [HasPermission(AuthorizePermissions.Administration.ManageUsers)]
    public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, CancellationToken ct)
       => FromResult(await _mediator.Send(new ChangeUserStatusCommandRequest(id), ct));
    #endregion
}