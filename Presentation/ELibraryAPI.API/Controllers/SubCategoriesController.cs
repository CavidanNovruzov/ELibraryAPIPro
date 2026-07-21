using ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;
using ELibraryAPI.Application.Features.Commands.SubCategory.DeleteSubCategory;
using ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;
using ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetAllSubCategory;
using ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class SubCategoriesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public SubCategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSubCategoryQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdSubCategoryQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Create([FromBody] CreateSubCategoryCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Update([FromBody] UpdateSubCategoryCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Delete([FromRoute] DeleteSubCategoryCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost("merge")]
    [HasPermission(AuthorizePermissions.Catalog.ManageCategories)]
    public async Task<IActionResult> Merge([FromBody] MergeSubCategoriesCommandRequest request)
        => FromResult(await _mediator.Send(request));
}