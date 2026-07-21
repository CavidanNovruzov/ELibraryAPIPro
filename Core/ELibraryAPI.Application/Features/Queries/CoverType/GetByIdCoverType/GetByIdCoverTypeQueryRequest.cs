using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;

public sealed record GetByIdCoverTypeQueryRequest(Guid Id) : IRequest<Result<GetByIdCoverTypeQueryResponse>>;
