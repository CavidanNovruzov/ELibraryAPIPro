using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;

public sealed record GetAllAuthorQueryRequest(
    int Page = 1,
    int Size = 10,
    string? SearchTerm = null
) : IRequest<Result<GetAllAuthorQueryResponse>>;