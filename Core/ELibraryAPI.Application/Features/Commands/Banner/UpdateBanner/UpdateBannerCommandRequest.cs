using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.UpdateBanner;

public sealed record UpdateBannerCommandRequest(
    Guid Id,
    string Title,
    string? FileName,      
    string? Base64File,
    string RedirectUrl,
    int Order,
    bool IsActive
) : IRequest<Result<UpdateBannerCommandResponse>>;
