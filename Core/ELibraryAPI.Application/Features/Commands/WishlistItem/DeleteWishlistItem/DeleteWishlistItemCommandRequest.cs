using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.DeleteWishlistItem;

public sealed record DeleteWishlistItemCommandRequest(Guid Id) : IRequest<Result>;
