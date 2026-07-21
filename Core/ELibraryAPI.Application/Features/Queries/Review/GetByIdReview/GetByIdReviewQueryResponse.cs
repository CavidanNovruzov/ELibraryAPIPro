namespace ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;

public sealed record GetByIdReviewQueryResponse(
    ReviewDetailDto Review
);

public sealed record ReviewDetailDto(
    Guid Id,
    Guid ProductId,
    string ProductTitle,
    string UserEmail,
    string Comment,
    int Rating,
    DateTime CreatedDate,
    int HelpfulCount
);
