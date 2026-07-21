using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.UpdateReview;

public sealed record UpdateReviewCommandRequest(
    Guid Id,
    string Comment,
    Guid ProductId,
    int Rating,
    Guid UserId
) : IRequest<Result<UpdateReviewCommandResponse>>;
