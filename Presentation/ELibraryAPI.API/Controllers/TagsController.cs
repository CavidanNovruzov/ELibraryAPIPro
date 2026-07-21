using ELibraryAPI.Application.Features.Commands.Tag.CreateTag;
using ELibraryAPI.Application.Features.Commands.Tag.DeleteTag;
using ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;
using ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class TagsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public TagsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTagQueryRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Create([FromBody] CreateTagCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Update([FromBody] UpdateTagCommandRequest request)
        => FromResult(await _mediator.Send(request));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Delete([FromRoute] DeleteTagCommandRequest request)
        => FromResult(await _mediator.Send(request));
}