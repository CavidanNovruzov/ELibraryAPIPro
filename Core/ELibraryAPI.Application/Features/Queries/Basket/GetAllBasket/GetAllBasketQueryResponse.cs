using System;
using System.Collections.Generic;

namespace ELibraryAPI.Application.Features.Queries.Basket.GetAllBasket;

public sealed record GetAllBasketQueryResponse(
    List<BasketListDto> Baskets,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record BasketListDto(Guid Id, Guid UserId, string UserEmail, decimal TotalPrice, int ItemCount);