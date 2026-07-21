using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.DeleteBanner;

public sealed record DeleteBannerCommandRequest(Guid Id) : IRequest<Result>;
