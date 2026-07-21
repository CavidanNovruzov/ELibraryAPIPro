using ELibraryAPI.Application.Features.Commands.Auth.ConfirmEmail;
using ELibraryAPI.Application.Features.Commands.Auth.ForgotPassword;
using ELibraryAPI.Application.Features.Commands.Auth.LoginUser;
using ELibraryAPI.Application.Features.Commands.Auth.LogoutUser;
using ELibraryAPI.Application.Features.Commands.Auth.RefreshToken;
using ELibraryAPI.Application.Features.Commands.Auth.RegistrUser;
using ELibraryAPI.Application.Features.Commands.Auth.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace ELibraryAPI.API.Controllers.Auth;

public sealed class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [AllowAnonymous]
    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register([FromBody] RegistrUserCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        if (UserId == null) return Unauthorized(); 
        return FromResult(await _mediator.Send(new LogoutUserCommandRequest(UserId.Value), ct));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest request, CancellationToken ct)
        => FromResult(await _mediator.Send(request, ct));
}