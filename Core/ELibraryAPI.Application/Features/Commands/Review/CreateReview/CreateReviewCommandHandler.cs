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
            return Result<CreateReviewCommandResponse>.Failure("Sistemə daxil olmuş istifadəçi tapılmadı.", ErrorType.Unauthorized);

        var reviewReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Review, Guid>();
        var reviewWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Review, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var orderItemReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderItem, Guid>(); 

        var productExists = await productReadRepo.ExistsAsync(x => x.Id == request.ProductId, false, ct);
        if (!productExists)
            return Result<CreateReviewCommandResponse>.Failure("Məhsul tapılmadı..", ErrorType.NotFound);

        if (request.Rating < 1 || request.Rating > 5)
            return Result<CreateReviewCommandResponse>.Failure("Reytinq 1 ilə 5 arasında olmalıdır.", ErrorType.BadRequest);

        var alreadyReviewed = await reviewReadRepo.ExistsAsync(
            x => x.ProductId == request.ProductId && x.UserId == currentUserGuid,
            false,
            ct);

        if (alreadyReviewed)
            return Result<CreateReviewCommandResponse>.Failure("Siz bu məhsula artıq rəy yazmısınız.", ErrorType.Conflict);

        var hasPurchased = await orderItemReadRepo.ExistsAsync(
            oi => oi.ProductId == request.ProductId
                  && oi.Order.UserId == currentUserGuid
                  && oi.Order.OrderStatus.Name == OrderStatusNames.Completed,
            false,
            ct);

        if (!hasPurchased)
            return Result<CreateReviewCommandResponse>.Failure(
                "Yalnız uğurla aldığınız və çatdırılan məhsullara rəy yaza bilərsiniz.",
                ErrorType.Forbidden); 

        var review = _mapper.Map<Domain.Entities.Concrete.Review>(request);

        review.IsApproved = false;

        await reviewWriteRepo.AddAsync(review, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateReviewCommandResponse>.Success(
            new CreateReviewCommandResponse(review.Id),
            "Rəy uğurla göndərildi və təsdiq gözləyir.");
    }
}