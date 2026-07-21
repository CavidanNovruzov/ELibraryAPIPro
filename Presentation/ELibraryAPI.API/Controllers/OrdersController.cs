using ELibraryAPI.Application.Features.Commands.Order.CancelOrder;
using ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;
using ELibraryAPI.Application.Features.Commands.Order.CreateOrder;
using ELibraryAPI.Application.Features.Commands.Order.DeleteOrder;
using ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;
using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;
using ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class OrdersController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Orders.View)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrderQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.View)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdOrderQueryRequest(id), ct));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Orders.UpdateStatus)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
          => FromResult(await _mediator.Send(new DeleteOrderCommandRequest(id), ct));

    [HttpPut("cancel/{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.Cancel)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new CancelOrderCommandRequest(id), ct));

    [HttpPatch("change-status")]
    [HasPermission(AuthorizePermissions.Orders.UpdateStatus)]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeOrderStatusCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
}