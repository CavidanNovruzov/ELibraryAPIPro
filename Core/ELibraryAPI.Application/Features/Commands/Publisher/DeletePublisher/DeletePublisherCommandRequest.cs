using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Publisher.DeletePublisher;

public sealed record DeletePublisherCommandRequest(Guid Id) : IRequest<Result>;
