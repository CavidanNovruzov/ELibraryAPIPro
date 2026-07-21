using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Review.GetByIdReview;

public sealed class GetByIdReviewQueryHandler : IRequestHandler<GetByIdReviewQueryRequest, Result<GetByIdReviewQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdReviewQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            return Result<GetByIdReviewQueryResponse>.Failure("Review was not found.");

        var dto = new ReviewDetailDto(
            reviewEntity.Id,
            reviewEntity.ProductId,
            reviewEntity.Product?.Title ?? string.Empty,
            reviewEntity.User?.Email ?? string.Empty,
            reviewEntity.Comment,
            reviewEntity.Rating,
            reviewEntity.CreatedDate,
            0 
        );

        return Result<GetByIdReviewQueryResponse>.Success(new GetByIdReviewQueryResponse(dto));
    }
}