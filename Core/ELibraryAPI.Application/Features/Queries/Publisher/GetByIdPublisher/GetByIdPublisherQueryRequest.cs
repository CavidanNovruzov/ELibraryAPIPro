using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;

public sealed record GetByIdPublisherQueryRequest(Guid Id) : IRequest<Result<GetByIdPublisherQueryResponse>>;
