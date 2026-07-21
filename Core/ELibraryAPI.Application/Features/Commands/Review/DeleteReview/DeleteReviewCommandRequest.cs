using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.DeleteReview;

public sealed record DeleteReviewCommandRequest(Guid Id) : IRequest<Result>;
