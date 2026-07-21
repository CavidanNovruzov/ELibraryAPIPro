

using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.ChangeBranchStatus;

public sealed record ChangeBranchStatusCommandRequest(
    Guid Id,
    bool IsActive
) : IRequest<Result<ChangeBranchStatusCommandResponse>>;