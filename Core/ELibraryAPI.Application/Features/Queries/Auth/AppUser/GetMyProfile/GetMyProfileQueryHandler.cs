using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;

public sealed class GetMyProfileQueryHandler
    : IRequestHandler<GetMyProfileQueryRequest, Result<GetMyProfileQueryResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public GetMyProfileQueryHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager, ICurrentUserService currentUserService)
        => (_userManager, _currentUserService) = (userManager, currentUserService);

    public async Task<Result<GetMyProfileQueryResponse>> Handle(
        GetMyProfileQueryRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result<GetMyProfileQueryResponse>.Failure("Sistemdə daxil edilməmisiniz.", ErrorType.Unauthorized);

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<GetMyProfileQueryResponse>.NotFound("İstifadəçi tapılmadı.");

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        return Result<GetMyProfileQueryResponse>.Success(
            new(user.Id, user.FirstName, user.LastName,
                user.UserName!, user.Email!, user.EmailConfirmed, roles));
    }
}
