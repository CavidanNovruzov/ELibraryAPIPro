using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Review.GetAllReview;

public sealed class GetAllReviewQueryHandler : IRequestHandler<GetAllReviewQueryRequest, Result<GetAllReviewQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllReviewQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllReviewQueryResponse>> Handle(GetAllReviewQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Review, Guid>()
            .GetAll(tracking: false);


        var reviews = await query
            .OrderByDescending(r => r.CreatedDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(r => new ReviewListDto(
                r.Id,
                r.ProductId,
                r.Product.Title,
                r.Product.Images.Where(i => i.IsMain).Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                r.User.Email,
                r.Comment,
                r.Rating,
                r.CreatedDate
            ))
            .ToListAsync(cancellationToken);

        return Result<GetAllReviewQueryResponse>.Success(new GetAllReviewQueryResponse(reviews));
    }
}