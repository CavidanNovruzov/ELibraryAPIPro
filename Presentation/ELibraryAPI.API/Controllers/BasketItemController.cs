using ELibraryAPI.Application.Features.Commands.BasketItem.ClearBasketItem;
using ELibraryAPI.Application.Features.Commands.BasketItem.CreateBasketItem;
using ELibraryAPI.Application.Features.Commands.BasketItem.DeleteBasketItem;
using ELibraryAPI.Application.Features.Commands.BasketItem.UpdateBasketItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/basket-items")]
[ApiController]
public sealed class BasketItemController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public BasketItemController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] CreateBasketItemCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateItemQuantity(Guid id, [FromBody] UpdateBasketItemQuantityRequest request, CancellationToken ct)
    {
        var command = request with { Id = id };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveItem(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteBasketItemCommandRequest(id), ct));

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> ClearMyBasket(CancellationToken ct)
        => FromResult(await _mediator.Send(new ClearBasketItemCommandRequest(UserId!.Value), ct));
}