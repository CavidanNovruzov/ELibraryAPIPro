using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.CoverType.CreateCoverType;

public sealed record CreateCoverTypeCommandRequest(
    string Name
) : IRequest<Result<CreateCoverTypeCommandResponse>>;
