using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.RegistrUser;

public record RegistrUserCommandRequest(
    string FirstName, 
    string LastName,
    string UserName,
    string Email,
    string Password
) : IRequest<Result<Guid>>;
