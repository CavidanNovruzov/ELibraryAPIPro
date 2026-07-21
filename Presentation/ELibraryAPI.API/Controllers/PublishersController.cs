using ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;
using ELibraryAPI.Application.Features.Commands.Publisher.DeletePublisher;
using ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;
using ELibraryAPI.Application.Features.Queries.Publisher.GetAllPublisher;
using ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class PublishersController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPublisherQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdPublisherQueryRequest (id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Catalog.ManagePublishers)]
    public async Task<IActionResult> Create([FromBody] CreatePublisherCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
    
    [HttpPut]
    [HasPermission(AuthorizePermissions.Catalog.ManagePublishers)]
    public async Task<IActionResult> Update([FromBody] UpdatePublisherCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Catalog.ManagePublishers)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeletePublisherCommandRequest (id), ct));
}