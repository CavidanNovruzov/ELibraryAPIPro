using ELibraryAPI.Application.Dtos;
using ELibraryAPI.Application.Features.Commands.Product.CreateProduct;
using ELibraryAPI.Application.Features.Commands.Product.DeleteProduct;
using ELibraryAPI.Application.Features.Commands.Product.DeleteProductImage;
using ELibraryAPI.Application.Features.Commands.Product.SetMainProductImage;
using ELibraryAPI.Application.Features.Commands.Product.SetProductActive;
using ELibraryAPI.Application.Features.Commands.Product.UploadProductImage;
using ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;
using ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;
using ELibraryAPI.Application.Features.Queries.Product.GetFeaturedProducts;
using ELibraryAPI.Application.Features.Queries.Product.GetNewArrivals;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class ProductsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdProductQueryRequest(id), ct));

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeatured(CancellationToken ct)
    => FromResult(await _mediator.Send(new GetFeaturedProductsQueryRequest(), ct));

    [HttpGet("new-arrivals")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNewArrivals(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetNewArrivalsQueryRequest(), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Books.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("{id}/images")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> UploadImages([FromRoute] Guid id, [FromForm] IFormFile[] files, CancellationToken ct)
    {
        var commandFiles = files.Select(file => new UploadFileDto(
            Content: file.OpenReadStream(),
            FileName: file.FileName
        )).ToList();

        var request = new UploadProductImageCommandRequest
        {
            ProductId = id, 
            Files = commandFiles
        };

        return FromResult(await _mediator.Send(request, ct));
    }

    [HttpPut]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Books.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteProductCommandRequest(id), ct));

    [HttpPatch("set-active")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> SetActive([FromBody] SetProductActiveCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPatch("{productId}/images/{imageId}/set-main")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> SetMainImage([FromRoute] Guid productId, [FromRoute] Guid imageId, CancellationToken ct)
        => FromResult(await _mediator.Send(new SetMainProductImageCommandRequest(productId, imageId), ct));

    [HttpDelete("images/{id}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> DeleteImage([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteProductImageCommandRequest(id), ct));
}
