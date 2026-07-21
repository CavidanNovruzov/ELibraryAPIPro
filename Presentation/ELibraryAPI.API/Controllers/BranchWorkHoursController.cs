using ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;
using ELibraryAPI.Application.Features.Commands.BranchWorkHours.DeleteBranchWorkHours;
using ELibraryAPI.Application.Features.Commands.BranchWorkHours.UpdateBranchWorkHours;
using ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetAllBranchWorkHours;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;


[Route("api/branches/{branchId:guid}/work-hours")]
public sealed class BranchWorkHoursController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public BranchWorkHoursController(IMediator mediator) => _mediator = mediator;
    [HttpGet]
    public async Task<IActionResult> GetByBranchId(Guid branchId, CancellationToken ct)
    {
        var query = new GetByBranchIdWorkHoursQueryRequest(branchId);
        return FromResult(await _mediator.Send(query, ct));
    }

    [HttpPost]
    [HasPermission(AuthorizePermissions.Branches.Update)] 
    public async Task<IActionResult> Create(Guid branchId, [FromBody] CreateBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        var command = request with { BranchId = branchId };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Branches.Update)]
    public async Task<IActionResult> Update(Guid branchId, Guid id, [FromBody] UpdateBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        var command = request with { Id = id, BranchId = branchId };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Branches.Update)]
    public async Task<IActionResult> Delete(Guid branchId, Guid id, CancellationToken ct)
    {
        return FromResult(await _mediator.Send(new DeleteBranchWorkHoursCommandRequest(id, branchId), ct));
    }
}