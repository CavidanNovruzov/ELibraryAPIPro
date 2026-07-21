using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetUserById;

public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQueryRequest, Result<GetUserByIdQueryResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result<GetUserByIdQueryResponse>> Handle(
        GetUserByIdQueryRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<GetUserByIdQueryResponse>.Failure("User not found.");

        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        var claims = (await _userManager.GetClaimsAsync(user))
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        return Result<GetUserByIdQueryResponse>.Success(
            new(user.Id, user.FirstName, user.LastName,
                user.UserName!, user.Email!, user.EmailConfirmed, roles, claims));
    }
}
