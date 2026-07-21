using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Branch.GetAllBranch;

public sealed record GetAllBranchQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllBranchQueryResponse>>;