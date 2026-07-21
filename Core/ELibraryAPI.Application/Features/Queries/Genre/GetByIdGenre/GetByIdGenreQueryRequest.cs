using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;

public sealed record GetByIdGenreQueryRequest(Guid Id) : IRequest<Result<GetByIdGenreQueryResponse>>;
