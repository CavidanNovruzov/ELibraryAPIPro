using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Publisher.GetAllPublisher;

public sealed record GetAllPublisherQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllPublisherQueryResponse>>;