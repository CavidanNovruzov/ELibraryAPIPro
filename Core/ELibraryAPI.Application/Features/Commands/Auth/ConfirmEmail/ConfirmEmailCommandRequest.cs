using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.ConfirmEmail;

public record ConfirmEmailCommandRequest(string UserId,string Token) : IRequest<Result>;

