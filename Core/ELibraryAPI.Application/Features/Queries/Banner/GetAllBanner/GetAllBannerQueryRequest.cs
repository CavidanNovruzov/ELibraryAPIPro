using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Banner.GetAllBanner;

public sealed record GetAllBannerQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllBannerQueryResponse>>;