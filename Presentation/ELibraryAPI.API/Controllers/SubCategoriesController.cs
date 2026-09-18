using ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;
using ELibraryAPI.Application.Features.Commands.SubCategory.DeleteSubCategory;
using ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;
using ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetAllSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/sub-categories")]
public class SubCategoriesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public SubCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSubCategoryQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdSubCategoryQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Create([FromBody] CreateSubCategoryCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSubCategoryCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteSubCategoryCommandRequest(id), ct));

    [HttpPost("merge")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Merge([FromBody] MergeSubCategoriesCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
}