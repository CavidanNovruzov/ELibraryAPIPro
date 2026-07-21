using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Banner.CreateBanner;

public sealed record CreateBannerCommandRequest(
    string Title,
    string FileName,
    string Base64File,
    string RedirectUrl,
    int Order,        
    bool IsActive
) : IRequest<Result<CreateBannerCommandResponse>>;
