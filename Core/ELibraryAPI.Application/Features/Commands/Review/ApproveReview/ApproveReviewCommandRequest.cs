using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.ApproveReview;

public sealed record ApproveReviewCommandRequest(Guid Id) : IRequest<Result<ApproveReviewCommandResponse>>;