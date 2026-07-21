using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.DeleteUser;

public sealed record DeleteUserCommandRequest(Guid Id) : IRequest<Result>;
