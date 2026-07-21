using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ELibraryAPI.Domain.Entities.Concrete.Auth;

namespace ELibraryAPI.Application.Features.Commands.Auth.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, Result>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result> Handle(ResetPasswordCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result.Failure("Invalid request.");

        var decodedToken = Uri.UnescapeDataString(request.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description).ToList());

        return Result.Success("Password reset successfully.");
    }
}
