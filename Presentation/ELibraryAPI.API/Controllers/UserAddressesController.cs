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
[Route("api/user-addresses")]
public class UserAddressesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public UserAddressesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllUserAddressQueryRequest(), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserAddressCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserAddressCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteUserAddressCommandRequest(id), ct));

    [HttpPatch("{id:guid}/set-default")]
    public async Task<IActionResult> SetDefault([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new SetDefaultAddressCommandRequest(id), ct));
}