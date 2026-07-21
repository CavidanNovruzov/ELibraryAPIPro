
using ELibraryAPI.Application.Features.Commands.CoverType.CreateCoverType;
using ELibraryAPI.Application.Features.Commands.CoverType.DeleteCoverType;
using ELibraryAPI.Application.Features.Commands.CoverType.UpdateCoverType;
using ELibraryAPI.Application.Features.Queries.CoverType.GetAllCoverType;
using ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/cover-types")]
public sealed class CoverTypesController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public CoverTypesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll([FromQuery] GetAllCoverTypeQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdCoverTypeQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Books.Edit)] 
    public async Task<IActionResult> Create([FromBody] CreateCoverTypeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCoverTypeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteCoverTypeCommandRequest(id), ct));
}

