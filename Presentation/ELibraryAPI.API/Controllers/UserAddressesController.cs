using ELibraryAPI.Application.Features.Commands.UserAddress.CreateUserAddress;
using ELibraryAPI.Application.Features.Commands.UserAddress.DeleteUserAddress;
using ELibraryAPI.Application.Features.Commands.UserAddress.SetDefaultAddress;
using ELibraryAPI.Application.Features.Commands.UserAddress.UpdateUserAddress;
using ELibraryAPI.Application.Features.Queries.UserAddress.GetAllUserAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
public class UserAddressesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public UserAddressesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var request = new GetAllUserAddressQueryRequest();
        return FromResult(await _mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserAddressCommandRequest request)
    {
        var command = request with { UserId = UserId.Value };
        return FromResult(await _mediator.Send(command));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserAddressCommandRequest request)
    {
        var command = request with { UserId = UserId.Value };
        return FromResult(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteUserAddressCommandRequest(id, UserId.Value);
        return FromResult(await _mediator.Send(command));
    }

    [HttpPatch("set-default/{id}")]
    public async Task<IActionResult> SetDefault([FromRoute] Guid id)
    {
        var command = new SetDefaultAddressCommandRequest(id, UserId.Value);
        return FromResult(await _mediator.Send(command));
    }
}