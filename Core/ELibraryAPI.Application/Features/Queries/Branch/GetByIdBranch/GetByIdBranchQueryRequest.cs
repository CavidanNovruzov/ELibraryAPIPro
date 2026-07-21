using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Branch.GetByIdBranch;

public sealed record GetByIdBranchQueryRequest(Guid Id) : IRequest<Result<GetByIdBranchQueryResponse>>;
