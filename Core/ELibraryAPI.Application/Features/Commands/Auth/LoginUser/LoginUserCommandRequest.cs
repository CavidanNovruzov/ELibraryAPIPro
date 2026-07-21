using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.LoginUser
{
    public sealed record LoginUserCommandRequest(
        string Login,
        string Password
    ) : IRequest<Result<LoginUserCommandResponse>>;
}
