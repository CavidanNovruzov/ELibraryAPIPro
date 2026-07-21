using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, Result<RefreshTokenCommandResponse>>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<RefreshTokenCommandResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken ct)
    {
        var result = await _authService.RefreshTokenAsync(new()
        {
            RefreshToken = request.RefreshToken,
        }, ct);

        if (!result.IsSuccess)
        {
            if (result.Errors != null && result.Errors.Any())
            {
                return Result<RefreshTokenCommandResponse>.Failure(result.Errors, result.Message ?? "Validation failed");
            }

            return Result<RefreshTokenCommandResponse>.Failure(result.Message ?? "Token refresh failed");
        }

        if (result.Data == null)
            return Result<RefreshTokenCommandResponse>.Failure(result.Message ?? "Token refresh failed");

        return Result<RefreshTokenCommandResponse>.Success(new(result.Data));
    }
}
