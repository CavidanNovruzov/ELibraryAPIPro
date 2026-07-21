using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorById;

public sealed record GetAuthorByIdQueryRequest(Guid Id) : IRequest<Result<GetAuthorByIdQueryResponse>>;