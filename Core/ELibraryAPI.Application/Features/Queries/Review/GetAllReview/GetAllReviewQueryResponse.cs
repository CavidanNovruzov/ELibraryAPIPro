namespace ELibraryAPI.Application.Features.Queries.Review.GetAllReview;

public sealed record GetAllReviewQueryResponse(List<ReviewListDto> Reviews);

public sealed record ReviewListDto(
    Guid Id,
    Guid ProductId,
    string ProductTitle,
    string ProductImageUrl,
    string FullName,
    string Comment,
    int Rating,
    DateTime CreatedDate
);