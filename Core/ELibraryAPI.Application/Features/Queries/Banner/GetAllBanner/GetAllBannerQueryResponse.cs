using System;
using System.Collections.Generic;

namespace ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;

public sealed record GetAllBannerQueryResponse(
    List<BannerListDto> Banners,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record BannerListDto(Guid Id, string ImageUrl, string RedirectUrl, string Title);