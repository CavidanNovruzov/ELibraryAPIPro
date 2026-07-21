using ELibraryAPI.Application.Features.Commands.PaymentMethod.CreatePaymentMethod;
using ELibraryAPI.Application.Features.Commands.PaymentMethod.DeletePaymentMethod;
using ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;
using ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class PaymentMethodsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPaymentMethodQueryRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost]
    [HasPermission(AuthorizePermissions.Finance.ManagePaymentMethods)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPut]
    [HasPermission(AuthorizePermissions.Finance.ManagePaymentMethods)]
    public async Task<IActionResult> Update([FromBody] UpdatePaymentMethodCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id}")]
    [HasPermission(AuthorizePermissions.Finance.ManagePaymentMethods)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeletePaymentMethodCommandRequest(id), ct));
}