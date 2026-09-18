using ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.DeleteShippingMethod;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;
using ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/shipping-methods")]
public class ShippingMethodsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ShippingMethodsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllShippingMethodQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Create([FromBody] CreateShippingMethodCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateShippingMethodCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteShippingMethodCommandRequest(id), ct));
}