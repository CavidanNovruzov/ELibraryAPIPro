using ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;
using ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;
using ELibraryAPI.Application.Features.Queries.UserSearchHistory.GetAllUserSearchHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
[Route("api/user-search-histories")]
public class UserSearchHistoriesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public UserSearchHistoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetAllUserSearchHistoryQueryRequest(), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserSearchHistoryCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteUserSearchHistoryCommandRequest(id), ct));
}