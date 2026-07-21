
namespace ELibraryAPI.Application.Features.Queries.Banner.GetActiveBanners;

public sealed record GetActiveBannersQueryResponse(
    Guid Id,
    string Title,
    string ImageUrl,
    int Order);
