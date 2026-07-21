using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommandRequest, Result>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result> Handle(ChangePasswordCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result.Failure("User not found.");

        var result = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description).ToList());

        return Result.Success("Password changed successfully.");
    }
}
