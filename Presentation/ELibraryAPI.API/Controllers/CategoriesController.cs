
using ELibraryAPI.Application.Features.Commands.Category.CreateCategory;
using ELibraryAPI.Application.Features.Commands.Category.DeleteCategory;
using ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;
using ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;
using ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;
using ELibraryAPI.Application.Features.Queries.Category.GetCategorySearch;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/categories")]
public sealed class CategoriesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdCategoryQueryRequest(id), ct));

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] GetCategorySearchQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)] 
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommandRequest request, CancellationToken ct)
    {
        return FromResult(await _mediator.Send(request, ct));
    }

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommandRequest request, CancellationToken ct)
    {
        var command = request with { Id = id };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteCategoryCommandRequest(id), ct));
}