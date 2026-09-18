using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;

public sealed class GetByIdReviewQueryHandler : IRequestHandler<GetByIdReviewQueryRequest, Result<GetByIdReviewQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetByIdReviewQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetByIdReviewQueryResponse>> Handle(GetByIdReviewQueryRequest request, CancellationToken cancellationToken)
    {
        var reviewEntity = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Review, Guid>()
            .GetAll(tracking: false)
            .Include(r => r.Product)
            .Include(r => r.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reviewEntity == null)
            return Result<GetByIdReviewQueryResponse>.Failure("Rəy tapılmadı.");

        if (!reviewEntity.IsApproved && reviewEntity.UserId != _currentUserService.UserGuid && !_currentUserService.IsAdmin)
            return Result<GetByIdReviewQueryResponse>.Failure("Bu rəy hələ təsdiqlənməyib və ya baxmaq üçün icazəniz yoxdur.");

        var dto = new ReviewDetailDto(
            reviewEntity.Id,
            reviewEntity.ProductId,
            reviewEntity.Product?.Title ?? string.Empty,
            reviewEntity.User?.UserName ?? string.Empty, 
            reviewEntity.Comment,
            reviewEntity.Rating,
            reviewEntity.CreatedDate,
            0
        );

        return Result<GetByIdReviewQueryResponse>.Success(new GetByIdReviewQueryResponse(dto));
    }
}