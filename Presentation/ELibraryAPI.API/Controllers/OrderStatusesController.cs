using ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;
using ELibraryAPI.Application.Features.Commands.OrderStatus.DeleteOrderStatus;
using ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;
using ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/order-statuses")]
public class OrderStatusesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public OrderStatusesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrderStatusQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Orders.UpdateStatus)]
    public async Task<IActionResult> Create([FromBody] CreateOrderStatusCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.UpdateStatus)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateOrderStatusCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Orders.UpdateStatus)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteOrderStatusCommandRequest(id), ct));
}