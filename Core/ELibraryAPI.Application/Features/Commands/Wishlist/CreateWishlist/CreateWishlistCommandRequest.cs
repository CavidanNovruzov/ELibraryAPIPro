using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.CreateWishlist;

public sealed record CreateWishlistCommandRequest() : IRequest<Result<CreateWishlistCommandResponse>>;
