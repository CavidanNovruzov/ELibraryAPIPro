using ELibraryAPI.Application.Features.Commands.Stock.CreateStock;
using ELibraryAPI.Application.Features.Commands.Stock.DeleteStock;
using ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;
using ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;
using ELibraryAPI.Application.Features.Queries.Stock.GetStockByProductId;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using ELibraryAPI.Infrastructure.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class StocksController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public StocksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Inventory.ViewStock)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllStockQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpGet("by-product/{productId}")]
    [HasPermission(AuthorizePermissions.Inventory.ViewStock)]
    public async Task<IActionResult> GetByProductId([FromRoute] GetStockByProductIdQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Create([FromBody] CreateStockCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Update([FromBody] UpdateStockCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Inventory.ManageStock)]
    public async Task<IActionResult> Delete([FromRoute] DeleteStockCommandRequest request)
        => FromResult(await _mediator.Send(request));
}