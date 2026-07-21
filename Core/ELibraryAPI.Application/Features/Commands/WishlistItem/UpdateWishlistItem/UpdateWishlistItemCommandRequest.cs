using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.UpdateWishlistItem;

public sealed record UpdateWishlistItemCommandRequest(
    Guid Id,
    Guid ProductId,
    Guid WishlistId
) : IRequest<Result<UpdateWishlistItemCommandResponse>>;
