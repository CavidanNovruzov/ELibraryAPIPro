using ELibraryAPI.Application.Features.Commands.Review.ApproveReview;
using ELibraryAPI.Application.Features.Commands.Review.CreateReview;
using ELibraryAPI.Application.Features.Commands.Review.DeleteReview;
using ELibraryAPI.Application.Features.Commands.Review.UpdateReview;
using ELibraryAPI.Application.Features.Queries.Review.GetAllReview;
using ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class ReviewsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ReviewsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllReviewQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdReviewQueryRequest (id), ct));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateReviewCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteReviewCommandRequest (id), ct));

    [HttpPatch("approve/{id}")]
    [HasPermission(AuthorizePermissions.Reviews.Moderate)]
    public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new ApproveReviewCommandRequest (id), ct));
}