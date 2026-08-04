using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.LoginUser
{
    public sealed record LoginUserCommandRequest(
        string Login,
        string Password
    ) : IRequest<Result<LoginUserCommandResponse>>;
}
