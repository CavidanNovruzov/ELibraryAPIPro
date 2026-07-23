using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.UpdateReview;

public sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommandRequest, Result<UpdateReviewCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService; 

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UpdateReviewCommandResponse>> Handle(UpdateReviewCommandRequest request, CancellationToken ct)
    {
        var reviewReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();

        var review = await reviewReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); 

        if (review == null)
            return Result<UpdateReviewCommandResponse>.Failure("Rəy tapılmadı.");

        if (review.UserId != _currentUserService.UserGuid)
            return Result<UpdateReviewCommandResponse>.Failure("Yalnız öz rəyinizi redaktə edə bilərsiniz.", ErrorType.Forbidden);

        if (request.Rating < 1 || request.Rating > 5)
            return Result<UpdateReviewCommandResponse>.Failure("Reytinq 1 ilə 5 arasında olmalıdır.");

        review.Rating = request.Rating;
        review.Comment = request.Comment;

        review.IsApproved = false;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateReviewCommandResponse>.Success(
            new UpdateReviewCommandResponse(review.Id),
            "Rəy yeniləndi və yenidən təsdiqə göndərildi.");
    }
}