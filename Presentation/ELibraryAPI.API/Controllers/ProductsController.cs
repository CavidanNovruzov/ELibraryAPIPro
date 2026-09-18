using ELibraryAPI.Application.Features.Commands.Product.CreateProduct;
using ELibraryAPI.Application.Features.Commands.Product.DeleteProduct;
using ELibraryAPI.Application.Features.Commands.Product.DeleteProductImage;
using ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;
using ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/products")]
public class ProductsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdProductQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Books.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteProductCommandRequest(id), ct));

    [HttpDelete("images/{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Delete)]
    public async Task<IActionResult> DeleteImage([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteProductImageCommandRequest(id), ct));
}