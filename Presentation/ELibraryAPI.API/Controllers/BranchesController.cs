
using ELibraryAPI.Application.Features.Commands.Branch.ChangeBranchStatus;
using ELibraryAPI.Application.Features.Commands.Branch.CreateBranch;
using ELibraryAPI.Application.Features.Commands.Branch.DeleteBranch;
using ELibraryAPI.Application.Features.Commands.Branch.UpdateBranch;
using ELibraryAPI.Application.Features.Queries.Branch.GetAllBranch;
using ELibraryAPI.Application.Features.Queries.Branch.GetByIdBranch;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/branches")]
public sealed class BranchesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public BranchesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll([FromQuery] GetAllBranchQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdBranchQueryRequest(id), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Branches.Create)]
    public async Task<IActionResult> Create([FromBody] CreateBranchCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Branches.Update)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBranchCommandRequest request, CancellationToken ct)
    {
        var command = request with { Id = id };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Branches.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteBranchCommandRequest(id), ct));

    [HttpPatch("{id:guid}/change-status")]
    [HasPermission(AuthorizePermissions.Branches.Update)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeBranchStatusCommandRequest request, CancellationToken ct)
    {
        var command = request with { Id = id };
        return FromResult(await _mediator.Send(command, ct));
    }
}