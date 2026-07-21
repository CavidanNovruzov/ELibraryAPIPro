using ELibraryAPI.Application.Responses;
using ELibraryAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ELibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid? UserId =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var guid) ? guid : null;

    protected IActionResult FromResult(Result result)
        => result.IsSuccess ? Ok(result) : MapError(result);

    protected IActionResult FromResult<T>(Result<T> result)
        => result.IsSuccess ? Ok(result) : MapError(result);

    private IActionResult MapError(Result result) => result.ErrorType switch
    {
        ErrorType.NotFound => NotFound(result),
        ErrorType.Unauthorized => Unauthorized(result),
        ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result),
        ErrorType.Conflict => Conflict(result),
        ErrorType.ValidationError => BadRequest(result),
        _ => BadRequest(result)
    };
}
