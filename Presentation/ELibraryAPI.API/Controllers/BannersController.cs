
using ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;
using ELibraryAPI.Application.Features.Commands.Banner.DeleteBanner;
using ELibraryAPI.Application.Features.Commands.Banner.UpdateBanner;
using ELibraryAPI.Application.Features.Queries.Banner.GetActiveBanners;
using ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/banners")]
public sealed class BannersController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public BannersController(IMediator mediator) => _mediator = mediator;
  
    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll([FromQuery] GetAllBannerQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetActiveBannersQueryRequest(), ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Marketing.ManageBanners)]
    public async Task<IActionResult> Create([FromBody] CreateBannerCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManageBanners)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBannerCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManageBanners)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteBannerCommandRequest(id), ct));
}