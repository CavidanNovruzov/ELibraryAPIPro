using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;
using ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;
using ELibraryAPI.Application.Features.Commands.WishlistItem.MoveToBasket;
using ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
public class WishlistItemsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public WishlistItemsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWishlistItemCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => FromResult(await _mediator.Send(new DeleteWishlistItemCommandRequest(id)));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWishlistItemCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost("move-to-basket")]
    public async Task<IActionResult> MoveToBasket([FromBody] MoveToBasketCommandRequest request)
        => FromResult(await _mediator.Send(request));
}