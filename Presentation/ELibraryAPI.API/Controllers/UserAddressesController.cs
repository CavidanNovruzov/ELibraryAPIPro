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
    public async Task<IActionResult> Create([FromBody] CreateUserAddressCommandRequest request, CancellationToken ct)
    {
        return FromResult(await _mediator.Send(request, ct));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserAddressCommandRequest request, CancellationToken ct)
    {
        return FromResult(await _mediator.Send(request, ct));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteUserAddressCommandRequest(id);
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpPatch("set-default/{id}")]
    public async Task<IActionResult> SetDefault([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new SetDefaultAddressCommandRequest(id);
        return FromResult(await _mediator.Send(command, ct));
    }
}