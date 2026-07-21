
using ELibraryAPI.Application.Features.Commands.Genre.CreateGenre;
using ELibraryAPI.Application.Features.Commands.Genre.DeleteGenre;
using ELibraryAPI.Application.Features.Commands.Genre.UpdateGenre;
using ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;
using ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/genres")]
public sealed class GenresController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll([FromQuery] GetAllGenreQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous] 
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdGenreQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Books.Edit)] 
    public async Task<IActionResult> Create([FromBody] CreateGenreCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGenreCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Books.Edit)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteGenreCommandRequest(id), ct));
}

