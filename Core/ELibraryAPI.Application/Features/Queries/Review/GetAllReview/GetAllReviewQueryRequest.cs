using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Review.GetAllReview;

public sealed record GetAllReviewQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllReviewQueryResponse>>;