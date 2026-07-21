using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.CoverType.UpdateCoverType;

public sealed record UpdateCoverTypeCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdateCoverTypeCommandResponse>>;
