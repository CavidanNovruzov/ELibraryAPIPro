using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.UpdateBranch;

public sealed record UpdateBranchCommandRequest(
    Guid Id,
    string Location,
    string Name,
    string Phone
) : IRequest<Result<UpdateBranchCommandResponse>>;
