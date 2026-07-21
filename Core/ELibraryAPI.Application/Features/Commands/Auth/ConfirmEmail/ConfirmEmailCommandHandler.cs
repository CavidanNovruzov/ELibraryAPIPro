using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommandRequest, Result>
{
    private readonly IAuthService _authService;

    public ConfirmEmailCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(ConfirmEmailCommandRequest request, CancellationToken ct)
    {
        return await _authService.ConfirmEmailAsync(new()
        {
            UserId = request.UserId,
            Token = request.Token
        },ct);
    }
}
