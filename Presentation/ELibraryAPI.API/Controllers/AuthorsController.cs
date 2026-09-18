using ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;
using ELibraryAPI.Application.Features.Commands.Author.DeleteAuthor;
using ELibraryAPI.Application.Features.Commands.Author.UpdateAuthor;
using ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;
using ELibraryAPI.Application.Features.Queries.Author.GetAuthorById;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/authors")]
public class AuthorsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public AuthorsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAuthorQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAuthorByIdQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Authors.Create)]
    public async Task<IActionResult> Create([FromBody] CreateAuthorCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Authors.Edit)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAuthorCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Authors.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteAuthorCommandRequest(id), ct));
}