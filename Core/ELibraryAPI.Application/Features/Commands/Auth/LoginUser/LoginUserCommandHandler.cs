using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.LoginUser;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, Result<LoginUserCommandResponse>>
{
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginUserCommandResponse>> Handle(
        LoginUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(new()
        {
            Login = request.Login,
            Password = request.Password
        }, cancellationToken);

        if(!result.IsSuccess)
        {
            if (result.Errors != null && result.Errors.Any())
            {
                return Result<LoginUserCommandResponse>.Failure(result.Errors);
            }

            return Result<LoginUserCommandResponse>.Failure(result.Message ?? "Giriş uğursuz oldu");
        }

        return Result<LoginUserCommandResponse>.Success(new LoginUserCommandResponse(result.Data!));
    }
}