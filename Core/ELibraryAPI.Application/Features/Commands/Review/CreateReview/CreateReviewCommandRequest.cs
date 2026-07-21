using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.CreateReview;

public sealed record CreateReviewCommandRequest(
    string Comment,
    Guid ProductId,
    int Rating
) : IRequest<Result<CreateReviewCommandResponse>>;
