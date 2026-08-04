using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;

public sealed record CreateUserSearchHistoryCommandRequest(
    string SearchQuery
) : IRequest<Result<CreateUserSearchHistoryCommandResponse>>;
