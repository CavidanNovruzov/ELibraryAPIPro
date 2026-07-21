using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Wishlist;

public sealed record GetCustomerWishlistQueryResponse(Guid WishlistId, List<WishlistItemDto> Items);

public sealed record WishlistItemDto(
    Guid ProductId,
    string Title,
    string AuthorName,     
    decimal OriginalPrice, 
    decimal? DiscountPrice,
    string ImageUrl,
    bool IsAvailable       
);