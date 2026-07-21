using ELibraryAPI.Application.Dtos.Auth;


namespace ELibraryAPI.Application.Features.Commands.Auth.LoginUser;

public class LoginUserCommandResponse
{
    public TokenResponse Token { get; set; } = null!;

    public LoginUserCommandResponse(TokenResponse token)
    {
        Token = token;
    }
}
