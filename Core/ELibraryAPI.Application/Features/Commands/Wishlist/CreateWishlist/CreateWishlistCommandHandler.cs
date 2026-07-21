using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;

public sealed class CreateWishlistCommandHandler : IRequestHandler<CreateWishlistCommandRequest, Result<CreateWishlistCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWishlistCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateWishlistCommandResponse>> Handle(CreateWishlistCommandRequest request, CancellationToken ct)
    {
        var userRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.AppUser, Guid>();
        var wishlistReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Wishlist, Guid>();
        var wishlistWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Wishlist, Guid>();

        var userExists = await userRepo.ExistsAsync(x => x.Id == request.UserId, tracking: false, ct: ct);
        if (!userExists)
            return Result<CreateWishlistCommandResponse>.Failure("User not found.");

        var alreadyHasWishlist = await wishlistReadRepo.ExistsAsync(x => x.UserId == request.UserId, tracking: false, ct: ct);
        if (alreadyHasWishlist)
            return Result<CreateWishlistCommandResponse>.Failure("Wishlist already exists for this user.");

        var wishlist = new Domain.Entities.Concrete.Wishlist
        {
            UserId = request.UserId
        };

        await wishlistWriteRepo.AddAsync(wishlist, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateWishlistCommandResponse>.Success(
            new CreateWishlistCommandResponse(wishlist.Id),
            "Wishlist created successfully.");
    }
}
