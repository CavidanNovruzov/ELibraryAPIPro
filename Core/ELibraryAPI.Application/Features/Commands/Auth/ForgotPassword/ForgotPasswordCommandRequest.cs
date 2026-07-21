using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.ForgotPassword;

public sealed record ForgotPasswordCommandRequest(string Email) : IRequest<Result>;
