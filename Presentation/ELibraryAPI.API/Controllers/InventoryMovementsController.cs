
using ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;
using ELibraryAPI.Application.Features.Queries.InventoryMovement.GetMovementsByProduct;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/inventory-movements")]
[Authorize] 
[HasPermission(AuthorizePermissions.Inventory.ViewStock)]
public class InventoryMovementsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public InventoryMovementsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Inventory.ViewMovements)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllInventoryMovementQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("product/{productId:guid}")]
    [HasPermission(AuthorizePermissions.Inventory.ViewMovements)]
    public async Task<IActionResult> GetByProduct([FromRoute] Guid productId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetMovementsByProductQueryRequest(productId), ct));
}

