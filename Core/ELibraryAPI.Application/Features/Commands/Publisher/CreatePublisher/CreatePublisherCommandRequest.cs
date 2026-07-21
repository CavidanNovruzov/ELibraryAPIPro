using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Publisher.CreatePublisher;

public sealed record CreatePublisherCommandRequest(
    string Description,
    string Name
) : IRequest<Result<CreatePublisherCommandResponse>>;
