using ELibraryAPI.Application.Features.Commands.Stock.CreateStock;
using ELibraryAPI.Application.Features.Commands.Stock.DeleteStock;
using ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;
using ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;
using ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/stocks")]
public class StocksController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public StocksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Inventory.ViewStock)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllStockQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("by-product/{productId:guid}")]
    [HasPermission(AuthorizePermissions.Inventory.ViewStock)]
    public async Task<IActionResult> GetByProductId([FromRoute] Guid productId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetStockByProductIdQueryRequest(productId), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Create([FromBody] CreateStockCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteStockCommandRequest(id), ct));
}