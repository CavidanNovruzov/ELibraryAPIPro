namespace ELibraryAPI.Application.Features.Queries.Review.GetAllReview;

public sealed record GetAllReviewQueryResponse(List<ReviewListDto> Reviews);

public sealed record ReviewListDto(
    Guid Id,
    Guid ProductId,
    string ProductTitle,
    string ProductImageUrl,
    string UserEmail,
    string Comment,
    int Rating,
    DateTime CreatedDate
);