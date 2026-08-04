using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;

public sealed record DeleteUserSearchHistoryCommandRequest(Guid Id) : IRequest<Result>;
