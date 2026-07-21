namespace ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;

public sealed record GetByIdPublisherQueryResponse(
    PublisherDetailDto Publisher
);

public sealed record PublisherDetailDto(
    Guid Id,
    string Name,
    string Description,
    int BookCount
);
