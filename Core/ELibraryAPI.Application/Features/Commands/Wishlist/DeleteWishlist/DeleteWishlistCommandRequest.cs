using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Wishlist.DeleteWishlist;

public sealed record DeleteWishlistCommandRequest(Guid Id) : IRequest<Result>;
