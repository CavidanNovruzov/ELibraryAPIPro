using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Basket.GetMyBasket;

public sealed record GetMyBasketQueryResponse(
   Guid BasketId,
   List<BasketItemDto> Items,
   decimal TotalAmount
);
public sealed record BasketItemDto(
    Guid BasketItemId,
    Guid ProductId,
    string ProductTitle,
    string CoverImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal SubTotal
);