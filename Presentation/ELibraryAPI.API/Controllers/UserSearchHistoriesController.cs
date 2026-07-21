using ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;
using ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;
using ELibraryAPI.Application.Features.Queries.UserSearchHistory.GetAllUserSearchHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibraryAPI.API.Controllers;

[Authorize]
public class UserSearchHistoriesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public UserSearchHistoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => FromResult(await _mediator.Send(new GetAllUserSearchHistoryQueryRequest()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserSearchHistoryCommandRequest request)
    {
        var command = request with { UserId = UserId.Value };
        return FromResult(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => FromResult(await _mediator.Send(new DeleteUserSearchHistoryCommandRequest(id, UserId.Value)));
}