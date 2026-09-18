
using ELibraryAPI.Application.Features.Commands.Basket.MoveToBasket;
using ELibraryAPI.Application.Features.Queries.Basket.GetAllBasket;
using ELibraryAPI.Application.Features.Queries.Basket.GetMyBasket;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
[Route("api/baskets")]
public sealed class BasketsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public BasketsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Basket.ViewAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllBasketQueryRequest(), ct));

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBasket(CancellationToken ct)
       => FromResult(await _mediator.Send(new GetMyBasketQueryRequest(), ct));

    [HttpPost("move-to-basket")] 
    public async Task<IActionResult> MoveToBasket([FromBody] MoveToBasketCommandRequest request, CancellationToken ct)
    => FromResult(await _mediator.Send(request, ct));

}