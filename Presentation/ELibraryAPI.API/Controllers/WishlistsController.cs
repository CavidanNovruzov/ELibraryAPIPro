using ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;
using ELibraryAPI.Application.Features.Commands.Wishlist.DeleteWishlist;
using ELibraryAPI.Application.Features.Queries.Wishlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
[Route("api/wishlists")]
public class WishlistsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public WishlistsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetByUserId(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetCustomerWishlistQueryRequest(), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteWishlistCommandRequest(id), ct));
}