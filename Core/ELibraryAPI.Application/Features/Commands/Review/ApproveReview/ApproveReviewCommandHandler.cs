using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.ApproveReview;

public sealed class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommandRequest, Result<ApproveReviewCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ApproveReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ApproveReviewCommandResponse>> Handle(ApproveReviewCommandRequest request, CancellationToken ct)
    {
        if (!_currentUserService.IsAdmin)
            return Result<ApproveReviewCommandResponse>.Failure("Bu əməliyyatı yerinə yetirmək üçün inzibatçı hüququnuz olmalıdır.", ErrorType.Forbidden);

        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();

        var review = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (review == null || review.IsDeleted)
            return Result<ApproveReviewCommandResponse>.Failure("Təsdiqlənəcək rəy tapılmadı.", ErrorType.NotFound);

        if (review.IsApproved)
            return Result<ApproveReviewCommandResponse>.Failure("Bu rəy artıq təsdiqlənib.", ErrorType.Conflict);

        review.IsApproved = true;

        await _unitOfWork.SaveAsync(ct);

        return Result<ApproveReviewCommandResponse>.Success(
            new ApproveReviewCommandResponse(review.Id),
            "Rəy uğurla təsdiqləndi və hər kəs üçün görünən oldu.");
    }
}