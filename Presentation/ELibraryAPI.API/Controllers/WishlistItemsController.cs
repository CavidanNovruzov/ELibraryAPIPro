using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;
using ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
[Route("api/wishlist-items")]
public class WishlistItemsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public WishlistItemsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWishlistItemCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWishlistItemCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteWishlistItemCommandRequest(id), ct));

    [HttpPost("move-to-basket")]
    public async Task<IActionResult> MoveToBasket([FromBody] MoveToBasketCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
}