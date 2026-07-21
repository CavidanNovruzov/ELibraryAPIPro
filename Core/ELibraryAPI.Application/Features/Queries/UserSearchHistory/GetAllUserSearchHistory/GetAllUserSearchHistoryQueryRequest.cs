using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.UserSearchHistory.GetAllUserSearchHistory;

public sealed record GetAllUserSearchHistoryQueryRequest : IRequest<Result<GetAllUserSearchHistoryQueryResponse>>;