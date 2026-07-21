using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.CoverType.GetAllCoverType;

public sealed record GetAllCoverTypeQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllCoverTypeQueryResponse>>;
