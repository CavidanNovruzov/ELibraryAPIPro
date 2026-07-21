using ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;
using ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;
using ELibraryAPI.Application.Features.Commands.OrderStatus.DeleteOrderStatus;
using ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class OrderStatusesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public OrderStatusesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrderStatusQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> Create([FromBody] CreateOrderStatusCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderStatusCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Administration.ManagePermissions)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteOrderStatusCommandRequest(id), ct));
}