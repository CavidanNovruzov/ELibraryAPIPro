using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.CreateTag;

public sealed record CreateTagCommandRequest(
    string Name
) : IRequest<Result<CreateTagCommandResponse>>;
