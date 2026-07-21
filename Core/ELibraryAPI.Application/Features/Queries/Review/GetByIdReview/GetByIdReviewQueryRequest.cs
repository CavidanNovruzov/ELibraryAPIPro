using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;

public sealed record GetByIdReviewQueryRequest(Guid Id) : IRequest<Result<GetByIdReviewQueryResponse>>;
