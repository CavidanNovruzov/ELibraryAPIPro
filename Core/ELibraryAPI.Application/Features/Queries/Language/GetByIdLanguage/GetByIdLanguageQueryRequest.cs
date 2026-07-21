using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Language.GetByIdLanguage;

public sealed record GetByIdLanguageQueryRequest(Guid Id) : IRequest<Result<GetByIdLanguageQueryResponse>>;
