using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Constants; 
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Review.CreateReview;

public sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommandRequest, Result<CreateReviewCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateReviewCommandResponse>> Handle(CreateReviewCommandRequest request, CancellationToken ct)
    {
        var currentUserGuid = _currentUserService.UserGuid;
        if (currentUserGuid == Guid.Empty)
            return Result<CreateReviewCommandResponse>.Failure("Authenticated user not found.", ErrorType.Unauthorized);

        var reviewReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();
        var reviewWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Review, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var orderItemReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderItem, Guid>(); 

        var productExists = await productReadRepo.ExistsAsync(x => x.Id == request.ProductId, false, ct);
        if (!productExists)
            return Result<CreateReviewCommandResponse>.Failure("Product not found.", ErrorType.NotFound);

        if (request.Rating < 1 || request.Rating > 5)
            return Result<CreateReviewCommandResponse>.Failure("Rating must be between 1 and 5.", ErrorType.BadRequest);

        var alreadyReviewed = await reviewReadRepo.ExistsAsync(
            x => x.ProductId == request.ProductId && x.UserId == currentUserGuid,
            false,
            ct);

        if (alreadyReviewed)
            return Result<CreateReviewCommandResponse>.Failure("You have already reviewed this product.", ErrorType.Conflict);

        var hasPurchased = await orderItemReadRepo.ExistsAsync(
            oi => oi.ProductId == request.ProductId
                  && oi.Order.UserId == currentUserGuid
                  && oi.Order.OrderStatus.Name == OrderStatusNames.Completed,
            false,
            ct);

        if (!hasPurchased)
            return Result<CreateReviewCommandResponse>.Failure(
                "You can only review products you have successfully purchased and received.",
                ErrorType.Forbidden); 

        var review = _mapper.Map<Domain.Entities.Concrete.Review>(request);

        review.IsApproved = false;

        await reviewWriteRepo.AddAsync(review, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateReviewCommandResponse>.Success(
            new CreateReviewCommandResponse(review.Id),
            "Review submitted successfully and is awaiting approval.");
    }
}