using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangeUserStatus;

public sealed record ChangeUserStatusCommandRequest(Guid Id) : IRequest<Result>;
