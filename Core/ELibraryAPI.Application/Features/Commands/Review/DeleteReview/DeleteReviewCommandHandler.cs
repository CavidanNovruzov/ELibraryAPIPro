using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.DeleteReview;

public sealed class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService; // libraff.az — [ICurrentUserService əlavə edildi]

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteReviewCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Review, Guid>();

        var review = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (review == null)
            return Result.Failure("Rəy tapılmadı.");

        // libraff.az — [Admin və ya Review sahibi yoxlanıldı]
        var isAdmin = _currentUserService.IsInRole("Admin");
        if (!isAdmin && review.UserId != _currentUserService.UserGuid)
            return Result.Failure("Bu rəyi silmək üçün icazəniz yoxdur.", ErrorType.Forbidden);

        writeRepo.Remove(review);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Rəy uğurla silindi.");
    }
}