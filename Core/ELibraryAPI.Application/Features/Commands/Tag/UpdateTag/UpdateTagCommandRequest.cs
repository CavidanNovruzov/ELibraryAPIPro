using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.UpdateTag;

public sealed record UpdateTagCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdateTagCommandResponse>>;
