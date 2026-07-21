using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetMyProfile;

public sealed class GetMyProfileQueryHandler
    : IRequestHandler<GetMyProfileQueryRequest, Result<GetMyProfileQueryResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public GetMyProfileQueryHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result<GetMyProfileQueryResponse>> Handle(
        GetMyProfileQueryRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<GetMyProfileQueryResponse>.Failure("User not found.");

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        return Result<GetMyProfileQueryResponse>.Success(
            new(user.Id, user.FirstName, user.LastName,
                user.UserName!, user.Email!, user.EmailConfirmed, roles));
    }
}
