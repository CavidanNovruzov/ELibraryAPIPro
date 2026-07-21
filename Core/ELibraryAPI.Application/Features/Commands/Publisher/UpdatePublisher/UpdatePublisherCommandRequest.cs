using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Publisher.UpdatePublisher;

public sealed record UpdatePublisherCommandRequest(
    Guid Id,
    string Description,
    string Name
) : IRequest<Result<UpdatePublisherCommandResponse>>;
