using ELibraryAPI.Application.Features.Commands.Campaign.AddProductToCampaign;
using ELibraryAPI.Application.Features.Commands.Campaign.CreateCampaign;
using ELibraryAPI.Application.Features.Commands.Campaign.DeleteCampaign;
using ELibraryAPI.Application.Features.Commands.Campaign.RemoveProductFromCampaign;
using ELibraryAPI.Application.Features.Commands.Campaign.ToggleCampaignStatus;
using ELibraryAPI.Application.Features.Commands.Campaign.UpdateCampaign;
using ELibraryAPI.Application.Features.Queries.Campaign.GetAllCampaign;
using ELibraryAPI.Application.Features.Queries.Campaign.GetByIdCampaign;
using ELibraryAPI.Application.Features.Queries.Campaign.GetProductsByCampaign;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/campaigns")]
public sealed class CampaignsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public CampaignsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCampaignQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetByIdCampaignQueryRequest(id), ct));

    [HttpGet("{campaignId:guid}/products")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductsByCampaign(
        [AsParameters] GetProductsByCampaignQueryRequest request,
        CancellationToken ct = default)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> Create([FromBody] CreateCampaignCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCampaignCommandRequest request, CancellationToken ct)
    {
        var command = request with { Id = id };
        return FromResult(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteCampaignCommandRequest(id), ct));

    [HttpPatch("{id:guid}/toggle-status")]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken ct)
    {
        return FromResult(await _mediator.Send(new ToggleCampaignStatusCommandRequest(id), ct));
    }

    [HttpPost("add-product")]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> AddProductToCampaign([FromBody] AddProductToCampaignCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("remove-product")]
    [HasPermission(AuthorizePermissions.Marketing.ManageCampaigns)]
    public async Task<IActionResult> RemoveProductFromCampaign([FromBody] RemoveProductFromCampaignCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
}