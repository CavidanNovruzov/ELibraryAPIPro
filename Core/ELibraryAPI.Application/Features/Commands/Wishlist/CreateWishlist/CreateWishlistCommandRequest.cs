using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;

public sealed record CreateWishlistCommandRequest(
    Guid UserId
) : IRequest<Result<CreateWishlistCommandResponse>>;
