using ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;
using ELibraryAPI.Application.Features.Commands.PromoCode.DeletePromoCode;
using ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;
using ELibraryAPI.Application.Features.Queries.PromoCode.CheckPromoCode;
using ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class PromoCodesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public PromoCodesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("check/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> Check([FromRoute] string code, CancellationToken ct)
        => FromResult(await _mediator.Send(new CheckPromoCodeQueryRequest(code), ct));

    [HttpGet]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPromoCodeQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Create([FromBody] CreatePromoCodeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Update([FromBody] UpdatePromoCodeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeletePromoCodeCommandRequest (id), ct));
}