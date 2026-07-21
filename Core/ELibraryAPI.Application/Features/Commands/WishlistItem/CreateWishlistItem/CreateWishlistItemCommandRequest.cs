using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.WishlistItem.CreateWishlistItem;

public sealed record CreateWishlistItemCommandRequest(
    Guid ProductId,
    Guid WishlistId
) : IRequest<Result<CreateWishlistItemCommandResponse>>;
