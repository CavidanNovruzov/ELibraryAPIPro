using ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.DeleteShippingMethod;
using ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;
using ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using ELibraryAPI.Infrastructure.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class ShippingMethodsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ShippingMethodsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Orders.View)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllShippingMethodQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Create([FromBody] CreateShippingMethodCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Update([FromBody] UpdateShippingMethodCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Orders.ManageShippingMethods)]
    public async Task<IActionResult> Delete([FromRoute] DeleteShippingMethodCommandRequest request)
        => FromResult(await _mediator.Send(request));
}