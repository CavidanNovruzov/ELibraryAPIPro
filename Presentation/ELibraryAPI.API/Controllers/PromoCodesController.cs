using ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;
using ELibraryAPI.Application.Features.Commands.PromoCode.DeletePromoCode;
using ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;
using ELibraryAPI.Application.Features.Queries.PromoCode.GetAllPromoCode;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Route("api/promo-codes")]
public class PromoCodesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public PromoCodesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPromoCodeQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Create([FromBody] CreatePromoCodeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePromoCodeCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request with { Id = id }, ct));

    [HttpDelete("{id:guid}")]
    [HasPermission(AuthorizePermissions.Marketing.ManagePromoCodes)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeletePromoCodeCommandRequest(id), ct));
}