using ELibraryAPI.Application.Features.Commands.Tag.CreateTag;
using ELibraryAPI.Application.Features.Commands.Tag.DeleteTag;
using ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;
using ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/tags")]
public class TagsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public TagsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTagQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Create([FromBody] CreateTagCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTagCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Catalog.ManageTags)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteTagCommandRequest(id), ct));
}