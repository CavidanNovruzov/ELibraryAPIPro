using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Language.GetAllLanguage;

public sealed record GetAllLanguageQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllLanguageQueryResponse>>;
