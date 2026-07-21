using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;

public sealed record GetAllGenreQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllGenreQueryResponse>>;
