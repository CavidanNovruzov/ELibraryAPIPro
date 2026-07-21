using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Basket.CreateBasket;

public sealed class CreateBasketCommandHandler
    : IRequestHandler<CreateBasketCommandRequest, Result<CreateBasketCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateBasketCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateBasketCommandResponse>> Handle(
        CreateBasketCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;

        if (userId == Guid.Empty)
            return Result<CreateBasketCommandResponse>.Failure(
                "Authenticated user not found.", ErrorType.Unauthorized);

        var basketReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Basket, Guid>();
        var basketWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Basket, Guid>();

        var hasActiveBasket = await basketReadRepo.ExistsAsync(
            x => x.UserId == userId, ct: ct);

        if (hasActiveBasket)
            return Result<CreateBasketCommandResponse>.Failure(
                "User already has an active shopping basket.");

        var basket = new Domain.Entities.Concrete.Basket
        {
            UserId = userId
        };

        await basketWriteRepo.AddAsync(basket, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateBasketCommandResponse>.Success(
            new CreateBasketCommandResponse(basket.Id));
    }
}
