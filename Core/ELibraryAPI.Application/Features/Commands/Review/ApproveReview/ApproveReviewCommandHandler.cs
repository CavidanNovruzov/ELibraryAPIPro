using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.ApproveReview;

public sealed class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommandRequest, Result<ApproveReviewCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ApproveReviewCommandResponse>> Handle(ApproveReviewCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();

        var review = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (review == null)
            return Result<ApproveReviewCommandResponse>.Failure("Review not found.");

        if (review.IsApproved)
            return Result<ApproveReviewCommandResponse>.Failure("Review is already approved.");

        review.IsApproved = true;

        await _unitOfWork.SaveAsync(ct);

        return Result<ApproveReviewCommandResponse>.Success(
            new ApproveReviewCommandResponse(review.Id),
            "Review has been approved and is now visible to everyone.");
    }
}