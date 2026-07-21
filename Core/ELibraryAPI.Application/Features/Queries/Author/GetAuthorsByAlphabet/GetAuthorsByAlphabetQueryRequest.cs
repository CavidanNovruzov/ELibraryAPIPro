using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;

public sealed record GetAuthorsByAlphabetQueryRequest(char Letter) : IRequest<Result<GetAuthorsByAlphabetQueryResponse>>;
