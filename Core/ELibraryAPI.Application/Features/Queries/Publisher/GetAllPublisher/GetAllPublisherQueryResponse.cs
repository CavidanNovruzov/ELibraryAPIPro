namespace ELibraryAPI.Application.Features.Queries.Publisher.GetAllPublisher;

public sealed record GetAllPublisherQueryResponse(
    List<PublisherListDto> Publishers,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record PublisherListDto(Guid Id, string Name, int BookCount);