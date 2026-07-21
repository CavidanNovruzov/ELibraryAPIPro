using ELibraryAPI.Application.Features.Commands.Transaction.CompleteTransactionCallback;
using ELibraryAPI.Application.Features.Commands.Transaction.InitializeTransaction;
using ELibraryAPI.Application.Features.Commands.Transaction.SyncTransactionStatus;
using ELibraryAPI.Application.Features.Queries.Transaction.GetAllTransaction;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Infrastructure.Security.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

public class TransactionsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [HasPermission(AuthorizePermissions.Finance.ViewTransactions)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTransactionQueryRequest request)
        => FromResult(await _mediator.Send(request));

 
    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromBody] InitializeTransactionCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromBody] CompleteTransactionCallbackCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("sync/{id:guid}")]
    public async Task<IActionResult> Sync([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new SyncTransactionStatusCommandRequest(id), ct));
}