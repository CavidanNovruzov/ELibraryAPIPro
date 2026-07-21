using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.CreateBranch;

public sealed record CreateBranchCommandRequest(
    string Location,
    string Name,
    string Phone
) : IRequest<Result<CreateBranchCommandResponse>>;
