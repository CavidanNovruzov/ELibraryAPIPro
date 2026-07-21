using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateProfile;

public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommandRequest, Result<UpdateProfileCommandResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public UpdateProfileCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result<UpdateProfileCommandResponse>> Handle(
        UpdateProfileCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<UpdateProfileCommandResponse>.Failure("User not found.");

        // UserName dəyişibsə unikallığı yoxla
        if (!string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userManager.FindByNameAsync(request.UserName);
            if (existing != null)
                return Result<UpdateProfileCommandResponse>.Failure("Username is already taken.");
        }

        user.FirstName = request.FirstName.Trim();
        user.LastName  = request.LastName.Trim();
        user.UserName  = request.UserName.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Result<UpdateProfileCommandResponse>.Failure(
                result.Errors.Select(e => e.Description).ToList());

        return Result<UpdateProfileCommandResponse>.Success(
            new UpdateProfileCommandResponse(user.Id, $"{user.FirstName} {user.LastName}"),
            "Profile updated successfully.");
    }
}
